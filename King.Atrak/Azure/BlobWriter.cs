namespace King.ATrak.Azure
{
    using King.Azure.Data;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Blob Writer
    /// </summary>
    public class BlobWriter
    {
        #region Members
        /// <summary>
        /// Container
        /// </summary>
        protected readonly IContainer container = null;
        #endregion

        #region Methods
        /// <summary>
        /// Initialize Container
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> Initialize()
        {
            var created = await this.container.CreateIfNotExists();

            if (created)
            {
                Trace.TraceInformation("Container created: '{0}'.", this.container.Name);
            }
            else
            {
                Trace.TraceInformation("Container already exists: '{0}'.", this.container.Name);
            }

            return created;
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