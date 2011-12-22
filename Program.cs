namespace Abc.Atrack
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Permissions;
    using System.Threading.Tasks;
    using Microsoft.Win32;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;

    class Program
    {
        static void Main(string[] args)
        {
            if (null == args || 3 != args.Length || args.Any(a => string.IsNullOrWhiteSpace(a)))
            {
                Console.WriteLine("Invalid Arguments: 1.) Folder, 2.) Container to upload to, 3.) Blob Storage Account");
            }
            else
            {
                try
                {
                    var folder = args[0];

                    if (Directory.Exists(folder))
                    {
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
                        Console.WriteLine(string.Format("Unknown Directory: '{0}'", folder));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("Completed.");
        }

        /// <summary>
        /// Upload Folder Contents
        /// </summary>
        /// <param name="folder">Folder</param>
        /// <param name="container">Container</param>
        private static void UploadFolderContents(string folder, CloudBlobContainer container)
        {
            var files = GetFiles(folder, new List<string>());
            Parallel.ForEach<string>(files, (file, state) =>
            {
                var objId = file.Replace(folder, string.Empty).ToLowerInvariant();
                var blob = container.GetBlobReference(objId);
                blob.UploadByteArray(File.ReadAllBytes(file));

                var contentType = ContentType(file);
                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    blob.Properties.ContentType = contentType;
                    blob.SetProperties();
                }
            });
        }

        /// <summary>
        /// Content Type
        /// </summary>
        /// <param name="filepath">File Path</param>
        /// <returns>Content Type</returns>
        public static string ContentType(string filepath)
        {
            var regPerm = new RegistryPermission(RegistryPermissionAccess.Read, "\\\\HKEY_CLASSES_ROOT");
            var classesRoot = Registry.ClassesRoot;

            var fi = new FileInfo(filepath);

            var dotExt = fi.Extension.ToLowerInvariant();

            var typeKey = classesRoot.OpenSubKey("MIME\\Database\\Content Type");

            foreach (var keyname in typeKey.GetSubKeyNames())
            {
                var curKey = classesRoot.OpenSubKey("MIME\\Database\\Content Type\\" + keyname);
                var extension = curKey.GetValue("Extension");
                if (null != extension && extension.ToString().ToLowerInvariant() == dotExt)
                {
                    return keyname;
                }
            }

            return null;
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

                files.AddRange(Directory.GetFiles(dir));
            }

            return files;
        }
    }
}