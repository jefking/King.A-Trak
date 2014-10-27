namespace King.ATrak.Azure
{
    using King.Azure.Data;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Blob Writer
    /// </summary>
    public class BlobWriter : IDataWriter
    {
        #region Members
        /// <summary>
        /// Container
        /// </summary>
        protected readonly IContainer container = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="name">Container Name</param>
        /// <param name="connectionString">Connection String</param>
        public BlobWriter(string name, string connectionString)
            : this(new Container(name, connectionString))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="container">Container</param>
        public BlobWriter(IContainer container)
        {
            if (null == container)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }
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
        public virtual async Task Store(IEnumerable<IStorageItem> items)
        {
            foreach (var item in items)
            {
                await item.Load();

                await this.container.Save(item.RelativePath, item.Data, item.ContentType);
            }
        }
        #endregion
    }
}