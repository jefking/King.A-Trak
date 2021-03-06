﻿namespace King.ATrak.Azure
{
    using King.Azure.Data;
    using Microsoft.WindowsAzure.Storage.Blob;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Blob Reader
    /// </summary>
    public class BlobReader : IDataLister
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
        public BlobReader(string name, string connectionString)
            : this(new Container(name, connectionString))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="container">Container</param>
        public BlobReader(IContainer container)
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
        /// List Blobs in Container
        /// </summary>
        /// <returns>Storage Items</returns>
        public virtual IEnumerable<IStorageItem> List()
        {
            var blobs = this.container.List(null, true);

            return null != blobs ? blobs.Select(b => new BlobItem(this.container, ((CloudBlockBlob)b).Name)) : null;
        }
        #endregion
    }
}