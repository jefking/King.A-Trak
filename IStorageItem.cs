namespace Abc.ATrak
{
    /// <summary>
    /// Interface for storage items
    /// </summary>
    public interface IStorageItem
    {
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
        #endregion

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
        #endregion
    }
}