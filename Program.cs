namespace BlobUploader
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

                        Upload(folder, container);
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

            Console.Read();
        }

        private static void Upload(string folder, CloudBlobContainer container)
        {
            var files = GetFiles(folder, new List<string>());
            Parallel.ForEach<string>(files, (file, state) =>
            {
                var objId = file.Replace(folder, string.Empty).ToLowerInvariant();
                var blob = container.GetBlobReference(objId);
                blob.UploadByteArray(File.ReadAllBytes(file));

                var contentType = GetMIMEType(file);
                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    blob.Properties.ContentType = contentType;
                    blob.SetProperties();
                }
            });
        }
        public static string GetMIMEType(string filepath)
        {
            var regPerm = new RegistryPermission(RegistryPermissionAccess.Read, "\\\\HKEY_CLASSES_ROOT");
            var classesRoot = Registry.ClassesRoot;

            var fi = new FileInfo(filepath);

            var dotExt = fi.Extension.ToLowerInvariant();

            RegistryKey typeKey = classesRoot.OpenSubKey("MIME\\Database\\Content Type");

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