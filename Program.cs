namespace Abc.ATrak
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Amazon;
    using Microsoft.WindowsAzure;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A-Trak synchronizer, for Azure Storage Blobs (containers) and folders
    /// </summary>
    public class Program
    {
        #region Methods
        /// <summary>
        /// Program Main Entry
        /// </summary>
        /// <param name="args">Program Arguments</param>
        public static void Main(string[] args)
        {
            try
            {
                var parameters = new Parameters(args);
                var factory = parameters.Process();

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

                if (!exists || to.MD5 != from.MD5 || to.MD5 == null)
                {
                    to.Save(from, exists);

                    Trace.WriteLine(string.Format("Synchronizing file: '{0}'.", from));
                }
                else
                {
                    Trace.WriteLine(string.Format("File '{0}' already exists at '{1}', synchronization avoided.", from.Path, to.Path));
                }

                to = null;
                from = null;
            });
        }
        #endregion
    }
}