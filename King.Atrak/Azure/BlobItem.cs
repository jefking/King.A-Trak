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
        /// <param name="path">Relative Path</param>
        public BlobItem(IContainer container, string path)
        {
            if (null == container)
            {
                throw new ArgumentNullException("container");
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("path");
            }

            this.RelativePath = path.StartsWith("/") ? path.Substring(1) : path;
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
            protected set;
        }

        /// <summary>
        /// Gets the Content Type
        /// </summary>
        public virtual string ContentType
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the Path
        /// </summary>
        public virtual string Path
        {
            get
            {
                return string.Format("{0}/{1}", this.container.Name, this.RelativePath);
            }
        }

        /// <summary>
        /// Gets the Relative Path
        /// </summary>
        public virtual string RelativePath
        {
            get;
            protected set;
        }

        /// <summary>
        /// Data
        /// </summary>
        public virtual byte[] Data
        {
            get;
            protected set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load MD5
        /// </summary>
        /// <returns>Task</returns>
        public async Task LoadMD5()
        {
            if (string.IsNullOrWhiteSpace(this.MD5))
            {
                var properties = await this.container.Properties(this.RelativePath);
                this.ContentType = properties.ContentType;
                this.MD5 = properties.ContentMD5;
            }
        }

        /// <summary>
        /// Load Data from storage
        /// </summary>
        /// <returns></returns>
        public virtual async Task Load()
        {
            if (null == this.Data)
            {
                this.Data = await this.container.Get(this.RelativePath);
            }
        }
        #endregion

    }
}