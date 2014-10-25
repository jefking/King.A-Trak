namespace King.ATrak
{
    using System;
    using System.Diagnostics;
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
        #endregion

        #region Methods
        /// <summary>
        /// Get Data
        /// </summary>
        /// <returns>Data for object</returns>
        public byte[] GetData()
        {
            //if (this.data == null)
            //{
            //    this.data = File.ReadAllBytes(this.Path); // < 2 gigs
            //    using (var createHash = System.Security.Cryptography.MD5.Create())
            //    {
            //        var hash = createHash.ComputeHash(this.data);
            //        this.MD5 = System.Convert.ToBase64String(hash);
            //    }
            //}

            //return this.data;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check to see if item exists
        /// </summary>
        /// <returns>Exists</returns>
        public bool Exists()
        {
            //return File.Exists(this.Path);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save Storage Item
        /// </summary>
        /// <param name="storageItem">Storage Item</param>
        /// <param name="exists">Exists</param>
        public void Save(IStorageItem storageItem, bool exists = false)
        {
            //Directory.CreateDirectory(System.IO.Path.GetDirectoryName(this.Path));

            //File.WriteAllBytes(this.Path, storageItem.GetData());
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete
        /// </summary>
        public void Delete()
        {
            //File.Delete(this.Path);
            //Trace.Write(string.Format("{0} deleted.", this.Path));
            throw new NotImplementedException();
        }
        #endregion
    }
}