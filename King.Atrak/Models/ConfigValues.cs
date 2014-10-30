namespace King.ATrak.Models
{
    /// <summary>
    /// Configuration Values
    /// </summary>
    public class ConfigValues : IConfigValues
    {
        #region Properties
        /// <summary>
        /// Create Snapshot
        /// </summary>
        public virtual bool CreateSnapshot
        {
            get;
            set;
        }
        
        /// <summary>
        /// Cache Control Duration (in seconds)
        /// </summary>
        public virtual int CacheControlDuration
        {
            get;
            set;
        }

        /// <summary>
        /// Echo
        /// </summary>
        public virtual bool Echo
        {
            get;
            set;
        }

        /// <summary>
        /// Sync Direction
        /// </summary>
        public virtual Direction Direction
        {
            get;
            set;
        }

        /// <summary>
        /// Data Source
        /// </summary>
        public IDataSource Source
        {
            get;
            set;
        }

        /// <summary>
        /// Data Destination
        /// </summary>
        public IDataSource Destination
        {
            get;
            set;
        }
        #endregion
    }
}