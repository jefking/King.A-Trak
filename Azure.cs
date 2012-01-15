namespace Abc.ATrak
{
    using System;
    using System.Diagnostics;
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
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes Cloud
        /// </summary>
        /// <param name="container">Container</param>
        /// <param name="objId">Object Id</param>
        public Azure(CloudBlobContainer container, string objId)
        {
            this.Path = objId;
            blob = container.GetBlobReference(objId);
            this.RelativePath = blob.Name;
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
                blob.FetchAttributes();
                this.ContentType = blob.Properties.ContentType;
                this.MD5 = blob.Metadata[MD5MetadataKey];
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
                blob.CreateSnapshot();

                Trace.WriteLine(string.Format("Created snapshot of blob: '{0}'.", blob.Uri));
            }

            if (source.Exists())
            {
                blob.Properties.ContentType = source.ContentType;
                // Currently there is a bug in the library that this isn't being stored or retrieved properly, this will be compatible when the new library comes out
                blob.Properties.ContentMD5 = source.MD5;
                blob.UploadByteArray(source.GetData());

                blob.Metadata[MD5MetadataKey] = source.MD5;
                blob.SetMetadata();
            }
        }

        /// <summary>
        /// Get Cloud Data
        /// </summary>
        /// <returns>Data for object</returns>
        public byte[] GetData()
        {
            return blob.DownloadByteArray();
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
    }
}