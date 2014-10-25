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
        /// Cache Control
        /// </summary>
        public virtual string CacheControl
        {
            get;
            set;
        }

        /// <summary>
        /// Folder
        /// </summary>
        public string Folder
        {
            get;
            set;
        }

        /// <summary>
        /// Container Name
        /// </summary>
        public string ContainerName
        {
            get;
            set;
        }

        /// <summary>
        /// Connection String
        /// </summary>
        public string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Sync Direction
        /// </summary>
        public virtual Direction SyncDirection
        {
            get;
            set;
        }
        #endregion
    }
}