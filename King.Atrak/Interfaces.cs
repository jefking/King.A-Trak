namespace King.ATrak
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for storage items
    /// </summary>
    public interface IStorageItem
    {
        #region Properties
        /// <summary>
        /// Gets the MD5 Hash
        /// </summary>
        string MD5
        {
            get;
        }

        /// <summary>
        /// Gets the Content Type
        /// </summary>
        string ContentType
        {
            get;
        }

        /// <summary>
        /// Gets the Path
        /// </summary>
        string Path
        {
            get;
        }

        /// <summary>
        /// Gets the Relative Path
        /// </summary>
        string RelativePath
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get Data
        /// </summary>
        /// <returns>Data for object</returns>
        byte[] GetData();

        /// <summary>
        /// Check to see if item exists
        /// </summary>
        /// <returns>Exists</returns>
        bool Exists();

        /// <summary>
        /// Save Storage Item
        /// </summary>
        /// <param name="storageItem">Storage Item</param>
        /// <param name="exists">Exists</param>
        void Save(IStorageItem storageItem, bool exists = false);

        /// <summary>
        /// Delete Storage Item
        /// </summary>
        void Delete();
        #endregion
    }
}