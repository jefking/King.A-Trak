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