namespace King.ATrak.Azure
{
    using King.Azure.Data;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
        public async Task<IEnumerable<IStorageItem>> List()
        {
            var blobs = this.container.List(null, true);

            var items = new List<IStorageItem>();
            foreach (var blob in blobs)
            {
                var properties = await this.container.Properties(blob.Uri.ToString());
                var item = new BlobItem(this.container, blob.Uri)
                {
                    MD5 = properties.ContentMD5,
                    ContentType = properties.ContentType,
                };
                items.Add(item);
            }

            return items;
        }
        #endregion
    }
}