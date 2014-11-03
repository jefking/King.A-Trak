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

        /// <summary>
        /// Echo deletions to destination
        /// </summary>
        protected readonly IEchoer echoer = null;
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
            if (null == config.Source)
            {
                throw new ArgumentNullException("source");
            }
            if (null == config.Destination)
            {
                throw new ArgumentNullException("destination");
            }
            if (Direction.Unknown == config.Direction)
            {
                throw new ArgumentException("Invalid Direction.");
            }

            switch (config.Direction)
            {
                case Direction.BlobToBlob:
                    this.lister = new BlobReader(config.Source.ContainerName, config.Source.ConnectionString);
                    this.writer = new BlobWriter(config.Destination.ContainerName, config.Destination.ConnectionString, config.CreateSnapshot);
                    break;
                case Direction.BlobToFolder:
                    this.lister = new BlobReader(config.Source.ContainerName, config.Source.ConnectionString);
                    this.writer = new FolderWriter(config.Destination.Folder);
                    break;
                case Direction.FolderToFolder:
                    this.lister = new FolderReader(config.Source.Folder);
                    this.writer = new FolderWriter(config.Destination.Folder);
                    break;
                case Direction.FolderToBlob:
                    this.lister = new FolderReader(config.Source.Folder);
                    this.writer = new BlobWriter(config.Destination.ContainerName, config.Destination.ConnectionString);
                    break;
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