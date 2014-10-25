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
            if (!arguments.Any() || arguments.Count() != 2)
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
            return new ConfigValues
            {

            };
        }
        #endregion
    }
}