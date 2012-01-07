namespace Abc.ATrak
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;

    /// <summary>
    /// A-Trak Blob Uploader
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Program Main Entry
        /// </summary>
        /// <param name="args">Program Arguments</param>
        public static void Main(string[] args)
        {
            if (null == args || 3 != args.Length || args.Any(a => string.IsNullOrWhiteSpace(a)))
            {
                Trace.WriteLine(string.Format("Invalid Arguments: {0}1.) Folder {0}2.) Container to upload to {0}3.) Blob Storage Account", Environment.NewLine));
            }
            else
            {
                var folder = args[0];

                try
                {
                    if (Directory.Exists(folder))
                    {
                        Trace.WriteLine(string.Format("Uploading from folder: '{0}'", folder));

                        var connectionString = args[2];
                        var account = CloudStorageAccount.Parse(connectionString);
                        var client = account.CreateCloudBlobClient();
                        client.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(5));
                        var container = client.GetContainerReference(args[1]);
                        container.CreateIfNotExist();

                        UploadFolderContents(folder, container);
                    }
                    else
                    {
                        Trace.WriteLine(string.Format("Unknown Directory: '{0}'", folder));
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
        /// Upload Folder Contents
        /// </summary>
        /// <param name="folder">Folder</param>
        /// <param name="container">Container</param>
        private static void UploadFolderContents(string folder, CloudBlobContainer container)
        {
            var path = folder;
            if (!path.EndsWith("\\"))
            {
                path += '\\';
            }

            var files = GetFiles(folder, new List<Disk>());
            Parallel.ForEach<IStorageItem>(files, (file, state) =>
            {
                Trace.WriteLine(string.Format("Processing file: '{0}'.", file));

                var objId = file.Path.Replace(path, string.Empty);
                IStorageItem blob = new Cloud(container, objId);
                var exists = blob.Exists();
                if (!exists)
                {
                    Trace.WriteLine(string.Format("Uploading new file: '{0}'.", file));
                }

                if (!exists || blob.MD5 != file.MD5)
                {
                    blob.Save(file, exists);

                    Trace.WriteLine(string.Format("Uploaded file: '{0}'.", file));
                }
                else
                {
                    Trace.WriteLine(string.Format("File '{0}' already exists at '{1}', upload avoided.", file.Path, blob.Path));
                }
            });
        }

        /// <summary>
        /// Get Files
        /// </summary>
        /// <param name="folder">Folder</param>
        /// <param name="files">Files</param>
        /// <returns>Files</returns>
        private static List<Disk> GetFiles(string folder, List<Disk> files)
        {
            foreach (var dir in Directory.GetDirectories(folder))
            {
                GetFiles(dir, files);
            }

            files.AddRange(Directory.GetFiles(folder).AsParallel().Select(f => new Disk(f)));

            return files;
        }
    }
}