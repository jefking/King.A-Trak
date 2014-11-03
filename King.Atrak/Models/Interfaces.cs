namespace King.ATrak.Models
{
    #region IConfigValues
    /// <summary>
    /// Configuration Values Interface
    /// </summary>
    public interface IConfigValues
    {
        #region Properties
        /// <summary>
        /// Create Snapshot
        /// </summary>
        bool CreateSnapshot
        {
            get;
        }

        /// <summary>
        /// Cache Control Duration (in seconds)
        /// </summary>
        int CacheControlDuration
        {
            get;
        }

        /// <summary>
        /// Echo
        /// </summary>
        bool Echo
        {
            get;
        }

        /// <summary>
        /// Sync Direction
        /// </summary>
        Direction Direction
        {
            get;
        }

        /// <summary>
        /// Data Source
        /// </summary>
        IDataSource Source
        {
            get;
        }

        /// <summary>
        /// Data Destination
        /// </summary>
        IDataSource Destination
        {
            get;
        }
        #endregion
    }
    #endregion

    #region IConfigValues
    /// <summary>
    /// Data Source Interface
    /// </summary>
    public interface IDataSource
    {
        #region Properties
        /// <summary>
        /// Folder
        /// </summary>
        string Folder
        {
            get;
        }

        /// <summary>
        /// Container Name
        /// </summary>
        string ContainerName
        {
            get;
        }

        /// <summary>
        /// Connection String
        /// </summary>
        string ConnectionString
        {
            get;
        }
        #endregion
    }
    #endregion
}