namespace King.ATrak.Windows
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Disk Storage Item
    /// </summary>
    public class FileItem : IStorageItem
    {
        #region Members
        /// <summary>
        /// Content Types
        /// </summary>
        protected readonly IContentTypes contentTypes = new ContentTypes();
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the Disk
        /// </summary>
        /// <param name="root">Root Path of File</param>
        /// <param name="path">Path of File</param>
        public FileItem(string root, string path)
        {
            if (string.IsNullOrWhiteSpace(root))
            {
                throw new ArgumentException("root");
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("path");
            }

            this.Path = path;
            this.RelativePath = path.Replace(root, string.Empty);
            this.RelativePath = this.RelativePath.StartsWith("\\") ? this.RelativePath.Substring(1) : this.RelativePath;
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
            get;
            protected set;
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
            await this.Load();
        }

        /// <summary>
        /// Load data, and calculate Hash
        /// </summary>
        public virtual async Task Load()
        {
            if (this.Data == null)
            {
                // Set Content Type
                this.ContentType = this.contentTypes.ContentType(this.Path);

                // Load Data
                this.Data = File.ReadAllBytes(this.Path); // < 2 gigs

                using (var createHash = System.Security.Cryptography.MD5.Create())
                {
                    var hash = createHash.ComputeHash(this.Data);

                    // Create Hash
                    this.MD5 = System.Convert.ToBase64String(hash);
                }
            }

            await new TaskFactory().StartNew(() => {});
        }

        /// <summary>
        /// Delete Item
        /// </summary>
        /// <returns>Task</returns>
        public virtual async Task Delete()
        {
            File.Delete(this.Path);

            await new TaskFactory().StartNew(() => { });
        }
        #endregion
    }
}