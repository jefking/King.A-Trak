namespace Abc.ATrak
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure;

    /// <summary>
    /// A-Trak synchronizer, for Azure Storage Blobs (containers) and folders
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Program Main Entry
        /// </summary>
        /// <param name="args">Program Arguments</param>
        public static void Main(string[] args)
        {
            if (null == args)
            {
                Trace.WriteLine("No arguments specified, please view help file for information on how to use A-Trak.");
            }
            else if (2 > args.Length || args.Any(a => string.IsNullOrWhiteSpace(a)))
            {
                Trace.WriteLine("You must specify a source and destination which you want to synchronize.");
            }
            else
            {
                var factory = new StorageFactory();

                try
                {
                    CloudStorageAccount account;
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (Directory.Exists(args[i]))
                        {
                            Trace.WriteLine(string.Format("Synchronizing folder: '{0}'", args[i]));

                            factory.AddDirectory(args[i]);
                        }
                        else if (CloudStorageAccount.TryParse(args[i], out account))
                        {
                            if (i + 1 < args.Length)
                            {
                                i++;

                                Trace.WriteLine(string.Format("Synchronizing container: '{0}'", args[i]));

                                factory.AddContainer(account, args[i]);
                            }
                            else
                            {
                                Trace.WriteLine("Storage Account Credentials must be coupled with container; please specify a container to synchronize to.");
                            }
                        }
                        else
                        {
                            Trace.Fail(string.Format("Unknown parameter: '{0}'", args[i]));
                            break;
                        }
                    }

                    if (factory.Validate())
                    {
                        SynchronizeContents(factory);
                    }
                    else
                    {
                        Trace.WriteLine("Failed to initialize; invalid parameters");
                    }
                }
                catch (Exception ex)
                {
                    Trace.Fail(ex.Message);
                }
            }

            Trace.WriteLine("Completed.");
        }

        /// <summary>
        /// Synchronize Contents
        /// </summary>
        /// <param name="factory">Storage Factory</param>
        private static void SynchronizeContents(StorageFactory factory)
        {
            Parallel.ForEach<IStorageItem>(factory.From(), (from, state) =>
            {
                Trace.WriteLine(string.Format("Processing file: '{0}'.", from));

                var to = factory.To(from);
                var exists = to.Exists();
                if (!exists)
                {
                    Trace.WriteLine(string.Format("Synchronizing new file: '{0}'.", from));
                }

                if (!exists || to.MD5 != from.MD5)
                {
                    to.Save(from, exists);

                    Trace.WriteLine(string.Format("Synchronizing file: '{0}'.", from));
                }
                else
                {
                    Trace.WriteLine(string.Format("File '{0}' already exists at '{1}', synchronization avoided.", from.Path, to.Path));
                }
            });
        }
    }
}