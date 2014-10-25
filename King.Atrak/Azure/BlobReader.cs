namespace King.ATrak.Azure
{
    using King.Azure.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BlobReader
    {
        protected readonly IContainer container = null;

        public IEnumerable<IStorageItem> List()
        {
            return null;
        }
    }
}