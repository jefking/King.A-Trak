namespace King.ATrak.Azure
{
    using King.Azure.Data;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Blob Storage Item
    /// </summary>
    public class BlobItem : IStorageItem
    {
        #region Members
        /// <summary>
        /// Cloud Blob
        /// </summary>
        protected readonly IContainer container = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the Azure
        /// </summary>
        /// <param name="container">Container</param>
        /// <param name="objId">Object Id</param>
        public BlobItem(IContainer container, Uri objId)
        {
            if (null == container)
            {
                throw new ArgumentNullException("container");
            }
            if (null == objId)
            {
                throw new ArgumentNullException("objId");
            }

            var path = objId.ToString();
            this.RelativePath = path.Substring(path.IndexOf(container.Name) + container.Name.Length + 2);
            this.container = container;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the MD5 Hash
        /// </summary>
        public virtual string MD5
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Content Type
        /// </summary>
        public virtual string ContentType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Path
        /// </summary>
        public virtual string Path
        {
            get
            {
                return this.container.Name + this.RelativePath;
            }
        }

        /// <summary>
        /// Gets the Relative Path
        /// </summary>
        public virtual string RelativePath
        {
            get;
            private set;
        }

        /// <summary>
        /// Data
        /// </summary>
        public virtual byte[] Data
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load Data from storage
        /// </summary>
        /// <returns></returns>
        public virtual async Task Load()
        {
            if (null == this.Data)
            {
                var path = string.Format("/{0}", this.RelativePath);
                var properties = await this.container.Properties(path);
                // Content Type
                this.ContentType = properties.ContentType;
                this.MD5 = properties.ContentMD5;

                // Data
                this.Data = await this.container.Get(path);
            }
        }

        /// <summary>
        /// Create Snapshot
        /// </summary>
        /// <param name="blob">Cloud Blob</param>
        //protected virtual void CreateSnapshot(ICloudBlob blob)
        //{
        //    var page = blob as CloudPageBlob;
        //    if (null != page)
        //    {
        //        page.CreateSnapshot();
        //    }

        //    var block = blob as CloudBlockBlob;
        //    if (null != block)
        //    {
        //        block.CreateSnapshot();
        //    }
        //    throw new NotImplementedException();
        //}
        #endregion
    }
}