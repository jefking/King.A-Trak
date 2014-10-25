namespace King.ATrak.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ConfigValues : IConfigValues
    {
        #region Properties
        public bool CreateSnapShot
        {
            get;
            set;
        }
        public string CacheControl
        {
            get;
            set;
        }
        /// <summary>
        /// Sync Direction
        /// </summary>
        public Direction SyncDirection
        {
            get;
            set;
        }
        #endregion
    }
}