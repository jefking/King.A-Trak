namespace King.ATrak.Windows
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

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

        #region Methods
        /// <summary>
        /// Initialize Folder
        /// </summary>
        public virtual void Initialize()
        {
            if (Directory.Exists(this.to))
            {
                Trace.TraceInformation("Directory already exists: '{0}'.", this.to);
            }
            else
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
        public virtual void Store(IEnumerable<IStorageItem> items)
        {
            foreach (var item in items)
            {

            }
        }
        #endregion
    }
}