namespace King.ATrak
{
    using System;
    using System.IO;

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
            get
            {
                return ContentTypes.ContentType(this.Path);
            }
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

        /// <summary>
        /// Data
        /// </summary>
        public byte[] Data
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        public void Load()
        {
            if (this.Data == null)
            {
                this.Data = File.ReadAllBytes(this.Path); // < 2 gigs
                using (var createHash = System.Security.Cryptography.MD5.Create())
                {
                    var hash = createHash.ComputeHash(this.Data);
                    this.MD5 = System.Convert.ToBase64String(hash);
                }
            }
        }
        #endregion
    }
}