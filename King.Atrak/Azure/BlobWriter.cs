namespace King.ATrak.Azure
{
    using King.Azure.Data;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Blob Writer
    /// </summary>
    public class BlobWriter
    {
        protected readonly IContainer container = null;

        public virtual async Task<bool> Initialize()
        {
            return await this.container.CreateIfNotExists();
        }

        public virtual void Store(IEnumerable<IStorageItem> items)
        {
            foreach (var item in items)
            {

            }
        }
    }
}