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
    public class Synchronizer
    {
        #region Members
        /// <summary>
        /// Folder Reader
        /// </summary>
        protected readonly FolderReader folderReader = null;

        /// <summary>
        /// Folder Writer
        /// </summary>
        protected readonly FolderWriter folderWriter = null;

        /// <summary>
        /// Blob Reader
        /// </summary>
        protected readonly BlobReader blobReader = null;

        /// <summary>
        /// Blob Writer
        /// </summary>
        protected readonly BlobWriter blobWriter = null;
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
                    this.blobReader = new BlobReader(config.ContainerName, config.ConnectionString);
                    this.folderWriter = new FolderWriter(config.Folder);
                    break;
                case Direction.FolderToBlob:
                    this.folderReader = new FolderReader(config.Folder);
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
        /// <param name="direction">Direction</param>
        /// <returns>Task</returns>
        public virtual async Task Run(Direction direction)
        {
            switch (direction)
            {
                case Direction.BlobToFolder:
                    Trace.TraceInformation("Retrieving blobs in container.");
                    var blobItems = this.blobReader.List();

                    Trace.TraceInformation("Initializing folder.");
                    this.folderWriter.Initialize();

                    Trace.TraceInformation("Storing blobs.");
                    await this.folderWriter.Store(blobItems);
                    break;
                case Direction.FolderToBlob:
                    Trace.TraceInformation("Retrieving files in folder.");
                    var folderItems = this.folderReader.List();

                    Trace.TraceInformation("Initializing container.");
                    await this.blobWriter.Initialize();

                    Trace.TraceInformation("Storing files.");
                    await this.blobWriter.Store(folderItems);
                    break;
                default:
                    throw new InvalidOperationException("Invalid Direction");
            }
        }
        #endregion
    }
}