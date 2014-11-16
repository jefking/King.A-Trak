namespace King.ATrak
{
    #region Direction
    /// <summary>
    /// Syncronization Direction
    /// </summary>
    public enum Direction : byte
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Folder to Blob
        /// </summary>
        FolderToBlob = 1,
        /// <summary>
        /// Blob to Folder
        /// </summary>
        BlobToFolder = 2,
        /// <summary>
        /// Blob to Blob
        /// </summary>
        BlobToBlob = 3,
        /// <summary>
        /// Folder to Folder
        /// </summary>
        FolderToFolder = 4,
    }
    #endregion
}