namespace King.ATrak
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    #region IStorageItem
    /// <summary>
    /// Interface for storage items
    /// </summary>
    public interface IStorageItem
    {
        #region Properties
        /// <summary>
        /// Gets the MD5 Hash
        /// </summary>
        string MD5
        {
            get;
        }

        /// <summary>
        /// Gets the Content Type
        /// </summary>
        string ContentType
        {
            get;
        }

        /// <summary>
        /// Gets the Path
        /// </summary>
        string Path
        {
            get;
        }

        /// <summary>
        /// Gets the Relative Path
        /// </summary>
        string RelativePath
        {
            get;
        }

        /// <summary>
        /// Get Data
        /// </summary>
        byte[] Data
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load object from data source
        /// </summary>
        /// <returns>Task</returns>
        Task Load();
        #endregion
    }
    #endregion

    #region IDataLister
    /// <summary>
    /// Data Listing Operation
    /// </summary>
    public interface IDataLister
    {
        #region Methods
        /// <summary>
        /// List Data Items
        /// </summary>
        /// <returns>Storage Items</returns>
        IEnumerable<IStorageItem> List();
        #endregion
    }
    #endregion

    #region ISynchronizer
    /// <summary>
    /// Data Synchronizer Interface
    /// </summary>
    public interface ISynchronizer
    {
        #region Methods
        /// <summary>
        /// Run Synchronization
        /// </summary>
        /// <returns>Task</returns>
        Task Run();
        #endregion
    }
    #endregion

    #region IDataWriter
    /// <summary>
    /// Data Writer
    /// </summary>
    public interface IDataWriter
    {

    }
    #endregion
}