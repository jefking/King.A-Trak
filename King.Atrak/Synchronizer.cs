namespace King.ATrak
{
    using King.ATrak.Azure;
    using King.ATrak.Models;
    using King.ATrak.Windows;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Data Synchronizer
    /// </summary>
    public class Synchronizer : ISynchronizer
    {
        #region Members
        /// <summary>
        /// Data Lister
        /// </summary>
        protected readonly IDataLister lister = null;

        /// <summary>
        /// Data Writer
        /// </summary>
        protected readonly IDataWriter writer = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="config">Configuration Values</param>
        public Synchronizer(IConfigValues config)
        {
            if (null == config)
            {
                throw new ArgumentNullException("config");
            }

            switch (config.SyncDirection)
            {
                case Direction.BlobToFolder:
                    this.lister = new BlobReader(config.ContainerName, config.ConnectionString);
                    this.writer = new FolderWriter(config.Folder);
                    break;
                case Direction.FolderToBlob:
                    this.lister = new FolderReader(config.Folder);
                    this.writer = new BlobWriter(config.ContainerName, config.ConnectionString);
                    break;
                default:
                    throw new ArgumentException("Invalid Direction.");
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run Synchronization
        /// </summary>
        /// <returns>Task</returns>
        public virtual async Task Run()
        {
            Trace.TraceInformation("Initializing storage...");
            await this.writer.Initialize();

            Trace.TraceInformation("Retrieving list...");
            var items = this.lister.List();

            Trace.TraceInformation("Storing...");
            await this.writer.Store(items);
        }
        #endregion
    }
}