namespace King.ATrak
{
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
        #endregion

        #region Methods
        /// <summary>
        /// Get Data
        /// </summary>
        /// <returns>Data for object</returns>
        byte[] GetData();

        /// <summary>
        /// Check to see if item exists
        /// </summary>
        /// <returns>Exists</returns>
        bool Exists();

        /// <summary>
        /// Save Storage Item
        /// </summary>
        /// <param name="storageItem">Storage Item</param>
        /// <param name="exists">Exists</param>
        void Save(IStorageItem storageItem, bool exists = false);

        /// <summary>
        /// Delete Storage Item
        /// </summary>
        void Delete();
        #endregion
    }
    #endregion

    #region IParameters
    /// <summary>
    /// Command Line Parameters
    /// </summary>
    public interface IParameters
    {
        #region Methods
        /// <summary>
        /// Process
        /// </summary>
        /// <returns></returns>
        StorageFactory Process();
        #endregion
    }
    #endregion
}