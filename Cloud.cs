namespace Abc.ATrak
{
    using System;
    using System.Diagnostics;
    using Microsoft.WindowsAzure.StorageClient;

    /// <summary>
    /// Cloud Storage Item
    /// </summary>
    public class Cloud : IStorageItem
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
        public Cloud(CloudBlobContainer container, string objId)
        {
            this.Path = objId;
            blob = container.GetBlobReference(objId);
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
                this.MD5 = blob.Metadata[ContentType];
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
        /// <param name="storageItem">Storage Item</param>
        /// <param name="exists">Exists</param>
        public void Save(IStorageItem storageItem, bool exists = false)
        {
            if (exists)
            {
                blob.CreateSnapshot();

                Trace.WriteLine(string.Format("Created snapshot of blob: '{0}'.", blob.Uri));
            }
            
            blob.Properties.ContentType = storageItem.ContentType;
            // Currently there is a bug in the library that this isn't being stored or retrieved properly, this will be compatible when the new library comes out
            blob.Properties.ContentMD5 = storageItem.MD5;
            blob.UploadByteArray(storageItem.GetData());

            blob.Metadata[MD5MetadataKey] = storageItem.MD5;
            blob.SetMetadata();
        }

        /// <summary>
        /// Get Cloud Data
        /// </summary>
        /// <returns>Data for object</returns>
        public byte[] GetData()
        {
            throw new NotImplementedException();
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
        #endregion
    }
}