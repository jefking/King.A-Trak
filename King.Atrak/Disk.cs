namespace Abc.ATrak
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Disk Storage Item
    /// </summary>
    public class Disk : IStorageItem
    {
        #region Members
        /// <summary>
        /// File Data
        /// </summary>
        private byte[] data = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the Disk
        /// </summary>
        /// <param name="root">Root Path of File</param>
        /// <param name="path">Path of File</param>
        public Disk(string root, string path)
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

            if (File.Exists(this.Path))
            {
                this.GetData();
                this.data = null;
            }
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
        #endregion

        #region Methods
        /// <summary>
        /// Get Data
        /// </summary>
        /// <returns>Data for object</returns>
        public byte[] GetData()
        {
            if (this.data == null)
            {
                this.data = File.ReadAllBytes(this.Path); // < 2 gigs
                using (var createHash = System.Security.Cryptography.MD5.Create())
                {
                    var hash = createHash.ComputeHash(this.data);
                    this.MD5 = System.Convert.ToBase64String(hash);
                }
            }

            return this.data;
        }

        /// <summary>
        /// Check to see if item exists
        /// </summary>
        /// <returns>Exists</returns>
        public bool Exists()
        {
            return File.Exists(this.Path);
        }

        /// <summary>
        /// Save Storage Item
        /// </summary>
        /// <param name="storageItem">Storage Item</param>
        /// <param name="exists">Exists</param>
        public void Save(IStorageItem storageItem, bool exists = false)
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(this.Path));

            File.WriteAllBytes(this.Path, storageItem.GetData());
        }

        /// <summary>
        /// Delete
        /// </summary>
        public void Delete()
        {
            File.Delete(this.Path);
            Trace.Write(string.Format("{0} deleted.", this.Path));
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