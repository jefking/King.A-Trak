namespace Abc.ATrak
{
    using Microsoft.WindowsAzure;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// A-Trak synchronizer, for Azure, folders and Amazon
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
                    Synchronize(factory);
                }
                else
                {
                    Trace.WriteLine("Failed to initialize; invalid parameters");
                }
            }
            catch (Exception ex)
            {
                Trace.Fail(ex.Message, ex.StackTrace);
            }

            Trace.TraceInformation("Completed.");
        }

        /// <summary>
        /// Synchronize Contents
        /// </summary>
        /// <param name="factory">Storage Factory</param>
        private static void Synchronize(StorageFactory factory)
        {
            Program.Push(factory);

            var delete = false;
            bool.TryParse(CloudConfigurationManager.GetSetting("Synchronize"), out delete);

            if (delete)
            {
                Trace.TraceInformation("Deleting items which are not in source.");

                Program.Delete(factory);
            }
        }

        /// <summary>
        /// Push data from source to destination
        /// </summary>
        /// <param name="factory">Storage Factory</param>
        private static void Push(StorageFactory factory)
        {
            Parallel.ForEach<IStorageItem>(factory.Source(), (from, state) =>
            {
                Trace.TraceInformation("Processing file: '{0}'.", from);

                var to = factory.To(from);
                var exists = to.Exists();
                if (!exists)
                {
                    Trace.TraceInformation("Synchronizing new file: '{0}'.", from);
                }

                if (!exists || to.MD5 != from.MD5 || to.MD5 == null)
                {
                    to.Save(from, exists);

                    Trace.TraceInformation("Synchronizing file: '{0}'.", from);
                }
                else
                {
                    Trace.TraceInformation("File '{0}' already exists at '{1}', synchronization avoided.", from.Path, to.Path);
                }

                to = null;
                from = null;
            });
        }

        /// <summary>
        /// Delete items from destination
        /// </summary>
        /// <param name="factory">Storage Factory</param>
        private static void Delete(StorageFactory factory)
        {
            var items = new HashSet<string>();
            foreach (var item in factory.Source())
            {
                items.Add(item.RelativePath);
            }

            Parallel.ForEach<IStorageItem>(factory.Destination(), (item, state) =>
            {
                if (!items.Contains(item.RelativePath))
                {
                    item.Delete();
                }
            });
        }
        #endregion
    }
}