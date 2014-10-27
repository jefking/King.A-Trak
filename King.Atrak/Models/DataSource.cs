namespace King.ATrak.Models
{
    /// <summary>
    /// Source
    /// </summary>
    public class DataSource : IDataSource
    {
        #region Properties
        /// <summary>
        /// Folder
        /// </summary>
        public virtual string Folder
        {
            get;
            set;
        }

        /// <summary>
        /// Container Name
        /// </summary>
        public virtual string ContainerName
        {
            get;
            set;
        }

        /// <summary>
        /// Connection String
        /// </summary>
        public virtual string ConnectionString
        {
            get;
            set;
        }
        #endregion
    }
}