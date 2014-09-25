namespace King.ATrak
{
    using Amazon.S3;
    using Amazon.S3.Model;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Storage Factory
    /// </summary>
    public class StorageFactory
    {
        #region Members
        /// <summary>
        /// From folder/container/bucket
        /// </summary>
        private string from;

        /// <summary>
        /// From Container
        /// </summary>
        private CloudBlobContainer fromContainer;

        /// <summary>
        /// From S3
        /// </summary>
        private IAmazonS3 fromClient;

        /// <summary>
        /// To Folder, Container, Bucket
        /// </summary>
        private string to;

        /// <summary>
        /// To Container
        /// </summary>
        private CloudBlobContainer toContainer;

        /// <summary>
        /// To S3
        /// </summary>
        private IAmazonS3 toClient;
        #endregion

        #region Methods
        /// <summary>
        /// Add Directory to Factory
        /// </summary>
        /// <param name="path">Path</param>
        public void AddDirectory(string path)
        {
            if (!path.EndsWith("\\"))
            {
                path += '\\';
            }

            if (string.IsNullOrWhiteSpace(this.from))
            {
                this.from = path;
            }
            else
            {
                this.to = path;

                if (!Directory.Exists(this.to))
                {
                    Directory.CreateDirectory(this.to);
                }
            }
        }

        /// <summary>
        /// Add Container to Factory
        /// </summary>
        /// <param name="account">Account</param>
        /// <param name="container">Container</param>
        public void AddContainer(CloudStorageAccount account, string container)
        {
            var client = account.CreateCloudBlobClient();
            if (string.IsNullOrWhiteSpace(this.from))
            {
                this.from = container;

                this.fromContainer = client.GetContainerReference(container);
                this.fromContainer.CreateIfNotExists();
            }
            else
            {
                this.to = container;

                this.toContainer = client.GetContainerReference(container);
                this.toContainer.CreateIfNotExists();
            }
        }

        /// <summary>
        /// Add Bucket
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="bucket">Bucket</param>
        public void AddBucket(IAmazonS3 client, string bucket)
        {
            if (string.IsNullOrWhiteSpace(this.from))
            {
                this.from = bucket;

                this.fromClient = client;
            }
            else
            {
                this.to = bucket;

                this.toClient = client;

                var request = new PutBucketRequest()
                {
                    BucketName = bucket,
                };

                var response = client.PutBucket(request);
            }
        }

        /// <summary>
        /// Validate Factory
        /// </summary>
        /// <returns>Is Valid</returns>
        public bool Validate()
        {
            return this.Validate(this.from, this.fromContainer, this.fromClient) && this.Validate(this.to, this.toContainer, this.toClient);
        }

        /// <summary>
        /// Validate Factory
        /// </summary>
        /// <param name="item">Item</param>
        /// <param name="container">Container</param>
        /// <returns>Is Valid</returns>
        private bool Validate(string item, CloudBlobContainer container, IAmazonS3 s3)
        {
            return (null == container && Directory.Exists(item))
                || (null != container && !string.IsNullOrWhiteSpace(item))
                || null != s3;
        }

        /// <summary>
        /// Storage items source
        /// </summary>
        /// <returns>Storage Items</returns>
        public IEnumerable<IStorageItem> Source()
        {
            return this.GetItems(this.fromContainer, this.fromClient, this.from);
        }

        /// <summary>
        /// Storage items destination
        /// </summary>
        /// <returns>Storage Items</returns>
        public IEnumerable<IStorageItem> Destination()
        {
            return this.GetItems(this.toContainer, this.toClient, this.to);
        }

        /// <summary>
        /// Get Items
        /// </summary>
        /// <param name="container">Container</param>
        /// <param name="client">Client</param>
        /// <param name="path">Path</param>
        /// <returns>Storage Items</returns>
        private IEnumerable<IStorageItem> GetItems(CloudBlobContainer container, IAmazonS3 client, string path)
        {
            if (null != container)
            {
                return container.ListBlobs(null, true, BlobListingDetails.None).Select(b => new Azure(container, b.Uri.ToString())).Where(c => c.Exists());
            }
            else if (null != client)
            {
                var request = new ListObjectsRequest()
                {
                    BucketName = path,
                };

                var response = client.ListObjects(request);
                return response.S3Objects.Select(s3 => new S3(client, path, s3.Key, s3.ETag));
            }
            else
            {
                return this.GetFiles(path, path, new List<IStorageItem>());
            }
        }

        /// <summary>
        /// Create to storage item
        /// </summary>
        /// <param name="existing">existing storage Item</param>
        /// <returns>Storage item</returns>
        public IStorageItem To(IStorageItem existing)
        {
            if (null != this.toContainer)
            {
                return new Azure(this.toContainer, existing.RelativePath);
            }
            else if (null != this.toClient)
            {
                return new S3(this.toClient, this.to, existing.RelativePath);
            }
            else
            {
                return new Disk(this.to, System.IO.Path.Combine(this.to, existing.RelativePath));
            }
        }

        /// <summary>
        /// Get Files
        /// </summary>
        /// <param name="root">Root Folder</param>
        /// <param name="folder">Folder</param>
        /// <param name="files">Files</param>
        /// <returns>Files</returns>
        private List<IStorageItem> GetFiles(string root, string folder, List<IStorageItem> files)
        {
            foreach (var dir in Directory.GetDirectories(folder))
            {
                this.GetFiles(root, dir, files);
            }

            files.AddRange(Directory.GetFiles(folder).AsParallel().Select(f => new Disk(root, f)));

            return files;
        }
        #endregion
    }
}