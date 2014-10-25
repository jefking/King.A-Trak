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