namespace Abc.ATrak
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;

    /// <summary>
    /// Storage Factory
    /// </summary>
    public class StorageFactory
    {
        #region Members
        /// <summary>
        /// From
        /// </summary>
        private string from;

        /// <summary>
        /// From Container
        /// </summary>
        private CloudBlobContainer fromContainer;

        /// <summary>
        /// To
        /// </summary>
        private string to;

        /// <summary>
        /// To Container
        /// </summary>
        private CloudBlobContainer toContainer;
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

            if (string.IsNullOrWhiteSpace(from))
            {
                from = path;
            }
            else
            {
                to = path;
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
            client.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(5));

            if (string.IsNullOrWhiteSpace(from))
            {
                from = container;

                fromContainer = client.GetContainerReference(container);
                fromContainer.CreateIfNotExist();
            }
            else
            {
                to = container;

                toContainer = client.GetContainerReference(container);
                toContainer.CreateIfNotExist();
            }
        }

        /// <summary>
        /// Validate Factory
        /// </summary>
        /// <returns>Is Valid</returns>
        public bool Validate()
        {
            return this.Validate(from, fromContainer) && this.Validate(to, toContainer);
        }

        /// <summary>
        /// Validate Factory
        /// </summary>
        /// <param name="item">Item</param>
        /// <param name="container">Container</param>
        /// <returns>Is Valid</returns>
        private bool Validate(string item, CloudBlobContainer container)
        {
            return (null == container && Directory.Exists(item))
                || (null != container && !string.IsNullOrWhiteSpace(item));
        }

        /// <summary>
        /// Storage items from source
        /// </summary>
        /// <returns>Storage Items</returns>
        public IEnumerable<IStorageItem> From()
        {
            if (null == fromContainer)
            {
                return GetFiles(from, from, new List<IStorageItem>());
            }
            else
            {
                var options = new BlobRequestOptions()
                {
                    UseFlatBlobListing = true,
                };
                return fromContainer.ListBlobs(options).Select(b => new Cloud(fromContainer, b.Uri.ToString())).Where(c => c.Exists());
            }
        }

        /// <summary>
        /// Create to storage item
        /// </summary>
        /// <param name="existing">existing storage Item</param>
        /// <returns>Storage item</returns>
        public IStorageItem To(IStorageItem existing)
        {
            return null == toContainer ? (IStorageItem)new Disk(to, System.IO.Path.Combine(to, existing.RelativePath)) : (IStorageItem)new Cloud(toContainer, existing.RelativePath);
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
                GetFiles(root, dir, files);
            }

            files.AddRange(Directory.GetFiles(folder).AsParallel().Select(f => new Disk(root, f)));

            return files;
        }
        #endregion
    }
}