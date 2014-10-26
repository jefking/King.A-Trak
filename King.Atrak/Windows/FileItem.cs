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
                throw new ArgumentNullException("root");
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("path");
            }

            this.Path = path;
            this.RelativePath = path.Replace(root, string.Empty);
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
        /// Load data, and calculate Hash
        /// </summary>
        public virtual async Task Load()
        {
            if (this.Data == null)
            {
                // Set Content Type
                this.ContentType = ContentTypes.ContentType(this.Path);

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
        #endregion
    }
}