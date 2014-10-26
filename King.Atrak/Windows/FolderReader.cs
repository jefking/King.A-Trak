namespace King.ATrak.Windows
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Folder Reader
    /// </summary>
    public class FolderReader : IDataLister
    {
        #region Members
        /// <summary>
        /// From
        /// </summary>
        protected readonly string from = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="from">From</param>
        public FolderReader(string from)
        {
            if (string.IsNullOrWhiteSpace(from))
            {
                throw new ArgumentException("from");
            }

            this.from = from;
        }
        #endregion

        #region Methods
        /// <summary>
        /// List items in folder
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IStorageItem> List()
        {
            return this.GetFiles(this.from, this.from, new List<IStorageItem>());
        }

        /// <summary>
        /// Get Files
        /// </summary>
        /// <param name="root">Root Folder</param>
        /// <param name="folder">Folder</param>
        /// <param name="files">Files</param>
        /// <returns>Files</returns>
        protected virtual IEnumerable<IStorageItem> GetFiles(string root, string folder, List<IStorageItem> files)
        {
            foreach (var dir in Directory.GetDirectories(folder))
            {
                this.GetFiles(root, dir, files);
            }

            files.AddRange(Directory.GetFiles(folder).Select(f => new FileItem(root, f)));

            return files;
        }
        #endregion
    }
}