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

        /// <summary>
        /// Create Snapshot
        /// </summary>
        protected readonly bool createSnapshot = false;

        /// <summary>
        /// Cache Control Duration
        /// </summary>
        protected readonly uint cacheControlDuration = 0;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlobWriter(string name, string connectionString, bool createSnapshot = false, int cacheControlDuration = 0)
            : this(new Container(name, connectionString), createSnapshot, cacheControlDuration)
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public BlobWriter(IContainer container, bool createSnapshot = false, int cacheControlDuration = 0)
        {
            if (null == container)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
            this.createSnapshot = createSnapshot;
            this.cacheControlDuration = (0 > cacheControlDuration) ? 0 : (uint)cacheControlDuration;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize Container
        /// </summary>
        /// <returns>Succcess</returns>
        public virtual async Task<bool> Initialize()
        {
            var created = await this.container.CreateIfNotExists();

            if (created)
            {
                Trace.TraceInformation("Container created: '{0}'.", this.container.Name);
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
                var path = item.RelativePath.Replace("\\", "/");

                var exists = await this.container.Exists(path);
                if (exists)
                {
                    await item.LoadMD5();
                    var existing = new BlobItem(container, path);
                    await existing.LoadMD5();
                    if (item.MD5 == existing.MD5)
                    {
                        Trace.TraceInformation("Synchronization avoided: '{0}'.", item.Path);
                        continue;
                    }

                    if (this.createSnapshot)
                    {
                        await this.container.Snapshot(path);
                    }
                }

                await item.LoadMD5();
                await item.Load();

                Trace.TraceInformation("Uploading to blob: '{0}'.", path);
                await this.container.Save(path, item.Data, item.ContentType);

                if (0 < this.cacheControlDuration)
                {
                    await this.container.SetCacheControl(path, this.cacheControlDuration);
                }
            }
        }
        #endregion
    }
}