namespace King.ATrak.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Folder Writer
    /// </summary>
    public class FolderWriter
    {
        #region Members
        /// <summary>
        /// Folder to
        /// </summary>
        protected readonly string to = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="to">To</param>
        public FolderWriter(string to)
        {
            if (string.IsNullOrWhiteSpace(to))
            {
                throw new ArgumentException("to");
            }

            this.to = to;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize Folder
        /// </summary>
        public virtual void Initialize()
        {
            if (!Directory.Exists(this.to))
            {
                Trace.TraceInformation("Creating directory: '{0}'.", this.to);

                Directory.CreateDirectory(this.to);

                Trace.TraceInformation("Created directory: '{0}'.", this.to);
            }
        }

        /// <summary>
        /// Store Items
        /// </summary>
        /// <param name="items">Items</param>
        public virtual async Task Store(IEnumerable<IStorageItem> items)
        {
            foreach (var item in items)
            {
                await item.Load();

                File.WriteAllBytes(Path.Combine(this.to, item.RelativePath), item.Data);
            }
        }
        #endregion
    }
}