namespace Abc.ATrak
{
    using Amazon;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using System;
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
        private readonly string[] arguments;

        /// <summary>
        /// Parameters Regex Statement
        /// </summary>
        private const string parametersRegexStatement = "[/](?<action>From|To)[ ](?<arguments>.[^/]*)";

        /// <summary>
        /// Values Regex Statement
        /// </summary>
        private static readonly string valuesRegexStatement = "[" + quote + "']*(?<value>.[^" + quote + "']*)[" + quote + "' ]*";

        /// <summary>
        /// Quote
        /// </summary>
        private const char quote = '\"';

        /// <summary>
        /// Backslash
        /// </summary>
        private const char backslash = '\\';

        /// <summary>
        /// Directory Match Regex Statement
        /// </summary>
        private const string directoryMatch = @"^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w ]*))+";
        #endregion

        #region Constructors
        /// <summary>
        /// Parameters
        /// </summary>
        /// <param name="arguments">Arguments</param>
        public Parameters(string[] arguments)
        {
            this.arguments = arguments;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Process Settings for Facotry
        /// </summary>
        /// <returns>Storage Factory</returns>
        public StorageFactory Process()
        {
            var factory = new StorageFactory();
            var from = ConfigurationManager.AppSettings["From"];
            var to = ConfigurationManager.AppSettings["To"];

            if (null != arguments)
            {
                var appendingFrom = true;
                var toParse = string.Join(" ", arguments);
                foreach (var arg in arguments)
                {
                    var val = arg.ToLowerInvariant();
                    if (val == "/from")
                    {
                        appendingFrom = true;
                    }
                    else if (val == "/to")
                    {
                        appendingFrom = false;
                    }
                    else if (appendingFrom)
                    {
                        from += string.Format("\"{0}\"", arg);
                    }
                    else
                    {
                        to += string.Format("\"{0}\"", arg);
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(from))
            {
                throw new InvalidOperationException("Specify where the data is coming from '/From' in arguments, or in AppSettings.");
            }
            else if (string.IsNullOrWhiteSpace(to))
            {
                throw new InvalidOperationException("Specify where the data is going to '/To' in arguments, or in AppSettings.");
            }
            else
            {
                var fromValues = Regex.Matches(from, valuesRegexStatement, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                switch (fromValues.Count)
                {
                    case 1:
                        var directory = fromValues[0].Groups["value"].Value;
                        if (Directory.Exists(directory))
                        {
                            Trace.WriteLine(string.Format("Synchronizing from folder: '{0}'", directory));

                            factory.AddDirectory(directory);
                        }
                        else
                        {
                            throw new InvalidOperationException("Directory does not exist.");
                        }
                        break;
                    case 2:
                        var accountArgument = fromValues[0].Groups["value"].Value;
                        var container = fromValues[1].Groups["value"].Value;
                        CloudStorageAccount account;
                        if (accountArgument == "UseDevelopmentStorage=true")
                        {
                            account = CloudStorageAccount.DevelopmentStorageAccount;
                        }
                        else if (!CloudStorageAccount.TryParse(accountArgument, out account))
                        {
                            account = null;
                        }

                        if (null != account)
                        {
                            if (!string.IsNullOrWhiteSpace(container))
                            {
                                Trace.WriteLine(string.Format("Synchronizing container: '{0}'", container));

                                factory.AddContainer(account, container);
                            }
                            else
                            {
                                Trace.WriteLine("Storage Account Credentials must be coupled with container; please specify a container to synchronize to.");
                            }
                        }
                        else
                        {
                            Trace.WriteLine("Storage Account Credentials is in invalid format.");
                        }
                        break;
                    case 3:
                        var accessKey = fromValues[0].Groups["value"].Value;
                        var secretAccessKey = fromValues[1].Groups["value"].Value;
                        var bucket = fromValues[2].Groups["value"].Value;
                        var client = AWSClientFactory.CreateAmazonS3Client(accessKey, secretAccessKey);
                        factory.AddBucket(client, bucket);
                        break;
                    default:
                        throw new InvalidOperationException(string.Format("Unknown parameters: '{0}'", from));
                }

                var toValues = Regex.Matches(to, valuesRegexStatement, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                switch (toValues.Count)
                {
                    case 1:
                        if (Regex.IsMatch(toValues[0].Groups["value"].Value, directoryMatch))
                        {
                            Trace.WriteLine(string.Format("Synchronizing folder: '{0}'", toValues[0].Groups["value"].Value));

                            factory.AddDirectory(toValues[0].Groups["value"].Value);
                        }
                        else
                        {
                            throw new InvalidOperationException("Directory is invalid.");
                        }
                        break;
                    case 2:
                        var accountArgument = toValues[0].Groups["value"].Value;
                        var container = toValues[1].Groups["value"].Value;
                        CloudStorageAccount account;
                        if (accountArgument == "UseDevelopmentStorage=true")
                        {
                            account = CloudStorageAccount.DevelopmentStorageAccount;
                        }
                        else if (!CloudStorageAccount.TryParse(accountArgument, out account))
                        {
                            account = null;
                        }

                        if (null != account)
                        {
                            if (!string.IsNullOrWhiteSpace(container))
                            {
                                Trace.WriteLine(string.Format("Synchronizing container: '{0}'", container));

                                factory.AddContainer(account, container);
                            }
                            else
                            {
                                Trace.WriteLine("Storage Account Credentials must be coupled with container; please specify a container to synchronize to.");
                            }
                        }
                        else
                        {
                            Trace.WriteLine("Storage Account Credentials is in invalid format.");
                        }
                        break;
                    case 3:
                        var accessKey = toValues[0].Groups["value"].Value;
                        var secretAccessKey = toValues[1].Groups["value"].Value;
                        var bucket = toValues[2].Groups["value"].Value;
                        var client = AWSClientFactory.CreateAmazonS3Client(accessKey, secretAccessKey);
                        factory.AddBucket(client, bucket);
                        break;
                    default:
                        throw new InvalidOperationException(string.Format("Unknown parameters: '{0}'", to));
                }
            }

            return factory;
        }
        #endregion
    }
}