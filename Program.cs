namespace Abc.ATrak
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Threading.Tasks;

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
                    Synchronize(factory);
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
        private static void Synchronize(StorageFactory factory)
        {
            Program.Push(factory);

            var delete = false;
            bool.TryParse(ConfigurationManager.AppSettings["Synchronize"], out delete);
            if (delete)
            {
                Trace.Write("Deleting items which are not in source.");

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