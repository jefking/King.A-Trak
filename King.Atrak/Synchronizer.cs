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
        /// Folder Writer
        /// </summary>
        protected readonly FolderWriter folderWriter = null;

        /// <summary>
        /// Blob Writer
        /// </summary>
        protected readonly BlobWriter blobWriter = null;

        /// <summary>
        /// Synchronization Direction
        /// </summary>
        protected readonly Direction direction = Direction.Unknown;
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

            this.direction = config.SyncDirection;

            switch (config.SyncDirection)
            {
                case Direction.BlobToFolder:
                    this.lister = new BlobReader(config.ContainerName, config.ConnectionString);
                    this.folderWriter = new FolderWriter(config.Folder);
                    break;
                case Direction.FolderToBlob:
                    this.lister = new FolderReader(config.Folder);
                    this.blobWriter = new BlobWriter(config.ContainerName, config.ConnectionString);
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
            Trace.TraceInformation("Retrieving items to be synchrnonized.");
            var items = this.lister.List();

            switch (this.direction)
            {
                case Direction.BlobToFolder:
                    Trace.TraceInformation("Initializing folder.");
                    this.folderWriter.Initialize();

                    Trace.TraceInformation("Storing blobs.");
                    await this.folderWriter.Store(items);
                    break;
                case Direction.FolderToBlob:
                    Trace.TraceInformation("Initializing container.");
                    await this.blobWriter.Initialize();

                    Trace.TraceInformation("Storing files.");
                    await this.blobWriter.Store(items);
                    break;
            }
        }
        #endregion
    }
}