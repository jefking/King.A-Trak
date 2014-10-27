namespace King.ATrak
{
    #region Direction
    /// <summary>
    /// Syncronization Direction
    /// </summary>
    public enum Direction : byte
    {
        Unknown = 0,
        FolderToBlob = 1,
        BlobToFolder = 2,
        BlobToBlob = 3,
        FolderToFolder = 4,
    }
    #endregion
}