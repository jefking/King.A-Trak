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
            await new TaskFactory().StartNew(() => { }); // TEMP

            switch (direction)
            {
                case Direction.BlobToFolder:
                    var blobReader = new BlobReader();
                    var folderWriter = new FolderWriter();
                    throw new NotImplementedException();
                case Direction.FolderToBlob:
                    var folderReader = new FolderReader();
                    var blobWriter = new BlobWriter();
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion
    }
}