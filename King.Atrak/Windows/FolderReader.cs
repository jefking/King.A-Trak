namespace King.ATrak.Windows
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class FolderReader
    {
        protected readonly string from = null;

        public IEnumerable<IStorageItem> List()
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
        private IEnumerable<IStorageItem> GetFiles(string root, string folder, List<IStorageItem> files)
        {
            foreach (var dir in Directory.GetDirectories(folder))
            {
                this.GetFiles(root, dir, files);
            }

            files.AddRange(Directory.GetFiles(folder).AsParallel().Select(f => new FileItem(root, f)));

            return files;
        }
    }
}