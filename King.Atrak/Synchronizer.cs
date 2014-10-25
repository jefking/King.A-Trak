namespace King.ATrak
{
    using King.ATrak.Azure;
    using King.ATrak.Models;
    using King.ATrak.Windows;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Data Synchronizer
    /// </summary>
    public class Synchronizer
    {
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
                    var blobReader = new BlobReader();
                    var blobItems = blobReader.List();

                    var folderWriter = new FolderWriter();
                    folderWriter.Initialize();

                    folderWriter.Store(blobItems);
                    break;
                case Direction.FolderToBlob:
                    var folderReader = new FolderReader();
                    var folderItems = folderReader.List();

                    var blobWriter = new BlobWriter();
                    await blobWriter.Initialize();

                    await blobWriter.Store(folderItems);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion
    }
}