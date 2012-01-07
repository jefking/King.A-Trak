namespace Abc.ATrak
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;

    /// <summary>
    /// A-Trak Blob Uploader
    /// </summary>
    public class Program
    {
        #region Members
        /// <summary>
        /// MD5 Key for Metadata
        /// </summary>
        private const string MD5MetadataKey = "MD5";
        #endregion

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

            var files = GetFiles(folder, new List<string>());
            Parallel.ForEach<string>(files, (file, state) =>
            {
                Trace.WriteLine(string.Format("Processing file: '{0}'.", file));

                var objId = file.Replace(path, string.Empty);
                var blob = container.GetBlobReference(objId);
                var exists = false;

                try
                {
                    blob.FetchAttributes();
                    exists = true;
                }
                catch (StorageClientException)
                {
                    Trace.WriteLine(string.Format("Uploading new file: '{0}'.", file));
                }

                var fileContents = File.ReadAllBytes(file);
                string md5 = null;
                using (var md5Hash = MD5.Create())
                {
                    var data = md5Hash.ComputeHash(fileContents);
                    md5 = System.Convert.ToBase64String(data);
                }

                if (!exists || blob.Metadata[MD5MetadataKey] != md5)
                {
                    if (!string.IsNullOrWhiteSpace(blob.Properties.ContentType))
                    {
                        blob.CreateSnapshot();

                        Trace.WriteLine(string.Format("Created snapshot of blob: '{0}'.", blob.Uri));
                    }
                    else
                    {
                        blob.Properties.ContentType = ContentTypes.ContentType(file);
                    }

                    // Currently there is a bug in the library that this isn't being stored or retrieved properly, this will be compatible when the new library comes out
                    blob.Properties.ContentMD5 = md5;
                    blob.UploadByteArray(fileContents);

                    blob.Metadata[MD5MetadataKey] = md5;
                    blob.SetMetadata();

                    Trace.WriteLine(string.Format("Uploaded file: '{0}'.", file));
                }
                else
                {
                    Trace.WriteLine(string.Format("File '{0}' already exists at '{1}', upload avoided.", file, blob.Uri));
                }
            });
        }

        /// <summary>
        /// Get Files
        /// </summary>
        /// <param name="folder">Folder</param>
        /// <param name="files">Files</param>
        /// <returns>Files</returns>
        private static List<string> GetFiles(string folder, List<string> files)
        {
            foreach (var dir in Directory.GetDirectories(folder))
            {
                GetFiles(dir, files);
            }

            files.AddRange(Directory.GetFiles(folder));

            return files;
        }
    }
}