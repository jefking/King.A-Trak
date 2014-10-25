namespace King.ATrak.Azure
{
    using King.Azure.Data;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Blob Reader
    /// </summary>
    public class BlobReader
    {
        #region Members
        /// <summary>
        /// Container
        /// </summary>
        protected readonly IContainer container = null;
        #endregion

        #region Methods
        /// <summary>
        /// List Blobs in Container
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IStorageItem> List()
        {
            var blobs = this.container.List(null, true);

            return blobs.Select(b => new BlobItem(this.container, b.Uri));
        }
        #endregion
    }
}