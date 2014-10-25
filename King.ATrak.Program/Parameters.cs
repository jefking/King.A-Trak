namespace King.ATrak
{
    using King.ATrak.Models;
    using Microsoft.WindowsAzure.Storage;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Parameters
    /// </summary>
    public class Parameters
    {
        #region Members
        /// <summary>
        /// Arguments
        /// </summary>
        protected readonly IReadOnlyList<string> arguments;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="arguments">Arguments</param>
        public Parameters(IReadOnlyList<string> arguments)
        {
            if (null == arguments)
            {
                throw new ArgumentNullException("arguments");
            }
            if (!arguments.Any() || arguments.Count() != 3)
            {
                throw new ArgumentException("Invalid parameter count.");
            }

            this.arguments = arguments;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Process Settings for Facotry
        /// </summary>
        /// <returns>Storage Factory</returns>
        public virtual IConfigValues Process()
        {
            var first = this.arguments.ElementAt(0).ToLowerInvariant();
            var connectionStringIndex = first.Contains("usedevelopmentstorage") || first.Contains("accountname") ? 0 : 1;

            return new ConfigValues
            {
                Folder = connectionStringIndex == 0 ? this.arguments.ElementAt(2) : this.arguments.ElementAt(0),
                ConnectionString = this.arguments.ElementAt(connectionStringIndex),
                ContainerName = this.arguments.ElementAt(connectionStringIndex + 1),
                SyncDirection = connectionStringIndex == 0 ? Direction.BlobToFolder : Direction.FolderToBlob,
            };
        }
        #endregion
    }
}