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

        /// <summary>
        /// Get Data
        /// </summary>
        byte[] Data
        {
            get;
        }
        #endregion

        #region Methods
        void Load();
        #endregion
    }
    #endregion
}