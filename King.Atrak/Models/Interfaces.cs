namespace King.ATrak.Models
{
    /// <summary>
    /// Configuration Values Interface
    /// </summary>
    public interface IConfigValues
    {
        #region Properties
        /// <summary>
        /// Sync Direction
        /// </summary>
        Direction SyncDirection
        {
            get;
        }

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
}