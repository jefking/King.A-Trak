namespace King.ATrak.Azure
{
    using King.Azure.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BlobWriter
    {
        protected readonly IContainer container = null;

        public virtual async Task<bool> Initialize()
        {
            return await this.container.CreateIfNotExists();
        }
    }
}