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
        #endregion
    }
}