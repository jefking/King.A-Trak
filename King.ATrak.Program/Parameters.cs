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
            if (!arguments.Any() || arguments.Count() > 4)
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
            var config = new ConfigValues()
            {
                CreateSnapshot = bool.Parse(ConfigurationManager.AppSettings["CreateSnapshot"]),
                //CacheControl = int.Parse(ConfigurationManager.AppSettings["CacheControl"]),
            };
            var source = new DataSource();
            var destination = new DataSource();

            switch (this.arguments.Count())
            {
                case 2:
                    source.Folder = this.arguments.ElementAt(0);
                    destination.Folder = this.arguments.ElementAt(1);
                    config.Direction = Direction.FolderToFolder;
                    break;
                case 3:
                    var first = this.arguments.ElementAt(0).ToLowerInvariant();
                    var connectionStringIndex = first.Contains("usedevelopmentstorage") || first.Contains("accountname") ? 0 : 1;

                    switch (connectionStringIndex)
                    {
                        case 0:
                            source.ConnectionString = this.arguments.ElementAt(0);
                            source.ContainerName = this.arguments.ElementAt(1);
                            destination.Folder = this.arguments.ElementAt(2);
                            config.Direction = Direction.BlobToFolder;
                            break;
                        case 1:
                            source.Folder = this.arguments.ElementAt(0);
                            destination.ConnectionString = this.arguments.ElementAt(1);
                            destination.ContainerName = this.arguments.ElementAt(2);
                            config.Direction = Direction.FolderToBlob;
                            break;
                    }
                    break;
                case 4:
                    source.ConnectionString = this.arguments.ElementAt(0);
                    source.ContainerName = this.arguments.ElementAt(1);
                    destination.ConnectionString = this.arguments.ElementAt(2);
                    destination.ContainerName = this.arguments.ElementAt(3);
                    config.Direction = Direction.BlobToBlob;
                    break;
            }
            config.Source = source;
            config.Destination = destination;
            
            return config;
        }
        #endregion
    }
}