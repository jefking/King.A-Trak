namespace Abc.ATrak
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.WindowsAzure.StorageClient;

    /// <summary>
    /// Azure Storage Item
    /// </summary>
    public class Azure : IStorageItem
    {
        #region Members
        /// <summary>
        /// MD5 Key for Metadata
        /// </summary>
        private const string MD5MetadataKey = "MD5";

        /// <summary>
        /// Cloud Blob
        /// </summary>
        private readonly CloudBlob blob;

        /// <summary>
        /// Create Snap Shot of blobs
        /// </summary>
        private static readonly bool createSnapShot = true;

        /// <summary>
        /// Cache Control
        /// </summary>
        private static readonly string cacheControl = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Staticly initializes Azure members
        /// </summary>
        static Azure()
        {
            bool.TryParse(ConfigurationManager.AppSettings["CreateSnapShot"], out createSnapShot);
            cacheControl = ConfigurationManager.AppSettings["CacheControl"];
            cacheControl = string.IsNullOrWhiteSpace(cacheControl) ? null : cacheControl;
        }

        /// <summary>
        /// Initializes a new instance of the Azure
        /// </summary>
        /// <param name="container">Container</param>
        /// <param name="objId">Object Id</param>
        public Azure(CloudBlobContainer container, string objId)
        {
            this.Path = objId;
            this.blob = container.GetBlobReference(objId);
            this.RelativePath = this.blob.Name;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the MD5 Hash
        /// </summary>
        public string MD5
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Content Type
        /// </summary>
        public string ContentType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Path
        /// </summary>
        public string Path
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Relative Path
        /// </summary>
        public string RelativePath
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check to see if item exists
        /// </summary>
        /// <returns>Exists</returns>
        public bool Exists()
        {
            try
            {
                this.blob.FetchAttributes(new BlobRequestOptions()
                {
                    Timeout = TimeSpan.FromMinutes(15)
                });

                this.ContentType = this.blob.Properties.ContentType;
                this.MD5 = this.blob.Metadata[MD5MetadataKey];
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        /// <summary>
        /// Save Storage Item
        /// </summary>
        /// <param name="source">Storage Item</param>
        /// <param name="exists">Exists</param>
        public void Save(IStorageItem source, bool exists = false)
        {
            if (exists)
            {
                if (createSnapShot)
                {
                    this.blob.CreateSnapshot();

                    Trace.WriteLine(string.Format("Created snapshot of blob: '{0}'.", this.blob.Uri));
                }
            }

            if (source.Exists())
            {
                this.blob.Properties.ContentType = source.ContentType;
                //// Currently there is a bug in the library that this isn't being stored or retrieved properly, this will be compatible when the new library comes out
                this.blob.Properties.ContentMD5 = source.MD5;
                this.blob.Properties.CacheControl = cacheControl;
                this.blob.UploadByteArray(source.GetData());

                if (!string.IsNullOrWhiteSpace(source.MD5))
                {
                    this.blob.Metadata[MD5MetadataKey] = source.MD5;
                    this.blob.SetMetadata();
                }
            }
        }

        /// <summary>
        /// Get Cloud Data
        /// </summary>
        /// <returns>Data for object</returns>
        public byte[] GetData()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                this.blob.DownloadToStream(stream, new BlobRequestOptions()
                {
                    Timeout = TimeSpan.FromMinutes(15)
                });

                bytes = stream.ToArray();
            }

            if (null != bytes)
            {
                if (string.IsNullOrWhiteSpace(this.MD5))
                {
                    using (var createHash = System.Security.Cryptography.MD5.Create())
                    {
                        var hash = createHash.ComputeHash(bytes);
                        this.MD5 = System.Convert.ToBase64String(hash);
                    }

                    if (createSnapShot)
                    {
                        blob.CreateSnapshot();
                    }

                    blob.Properties.ContentMD5 = this.MD5;
                    this.blob.Metadata[MD5MetadataKey] = this.MD5;
                    this.blob.SetMetadata();
                }
            }

            return bytes;
        }

        /// <summary>
        /// Delete
        /// </summary>
        public void Delete()
        {
            var options = new BlobRequestOptions()
            {
                DeleteSnapshotsOption = DeleteSnapshotsOption.IncludeSnapshots,
            };

            var deleted = this.blob.DeleteIfExists(options);
            if (deleted)
            {
                Trace.Write(string.Format("{0} deleted.", this.Path));
            }
        }

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>String reprensentation of object</returns>
        public override string ToString()
        {
            return string.Format("{0}", this.Path);
        }
        #endregion
    }
}