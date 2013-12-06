namespace Abc.ATrak
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Azure Storage Item
    /// </summary>
    public class Azure : IStorageItem
    {
        #region Members
        /// <summary>
        /// Cloud Blob
        /// </summary>
        private readonly CloudBlockBlob blob;

        /// <summary>
        /// Blob Request Options
        /// </summary>
        private readonly BlobRequestOptions options = new BlobRequestOptions()
        {
            ServerTimeout = TimeSpan.FromMinutes(15),
        };

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
            this.Path = objId.Replace('\\', '/');
            this.blob = container.GetBlockBlobReference(this.Path);
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
                this.blob.FetchAttributes(null, options);

                this.ContentType = this.blob.Properties.ContentType;
                this.MD5 = this.blob.Properties.ContentMD5;
                return this.blob.Exists();
            }
            catch (StorageException)
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
                    this.CreateSnapshot(this.blob);

                    Trace.WriteLine(string.Format("Created snapshot of blob: '{0}'.", this.blob.Uri));
                }
            }

            if (source.Exists())
            {
                this.blob.Properties.ContentType = source.ContentType;
                this.blob.Properties.ContentMD5 = source.MD5;
                this.blob.Properties.CacheControl = cacheControl;
                try
                {
                    using (var stream = new MemoryStream(source.GetData()))
                    {
                        this.blob.UploadFromStream(stream);
                    }
                }
                catch { }
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
                this.blob.DownloadToStream(stream, AccessCondition.GenerateEmptyCondition(), options);

                bytes = stream.ToArray();
            }

            if (null != bytes)
            {
                if (string.IsNullOrWhiteSpace(this.MD5))
                {
                    using (var createHash = System.Security.Cryptography.MD5.Create())
                    {
                        createHash.TransformBlock(bytes, 0, bytes.Length, null, 0);
                        createHash.TransformFinalBlock(new byte[0], 0, 0);
                        this.MD5 = System.Convert.ToBase64String(createHash.Hash);
                    }

                    if (createSnapShot)
                    {
                        this.CreateSnapshot(this.blob);
                    }

                    blob.Properties.ContentMD5 = this.MD5;
                    blob.SetProperties(null, options);
                }
            }

            return bytes;
        }

        /// <summary>
        /// Delete
        /// </summary>
        public void Delete()
        {
            var deleted = this.blob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
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

        /// <summary>
        /// Create Snapshot
        /// </summary>
        /// <param name="blob">Cloud Blob</param>
        private void CreateSnapshot(ICloudBlob blob)
        {
            var page = blob as CloudPageBlob;
            if (null != page)
            {
                page.CreateSnapshot();
            }

            var block = blob as CloudBlockBlob;
            if (null != block)
            {
                block.CreateSnapshot();
            }
        }
        #endregion
    }
}