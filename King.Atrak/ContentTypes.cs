namespace King.ATrak
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Permissions;

    /// <summary>
    /// Content Types
    /// </summary>
    public static class ContentTypes
    {
        #region Members
        /// <summary>
        /// Content Types
        /// </summary>
        protected readonly static IDictionary<string, string> types = new ConcurrentDictionary<string, string>();
        #endregion

        #region Methods
        /// <summary>
        /// Content Type
        /// </summary>
        /// <param name="filepath">File Path</param>
        /// <returns>Content Type</returns>
        public virtual static string ContentType(string filepath)
        {
            if (string.IsNullOrWhiteSpace(filepath))
            {
                throw new ArgumentException("filepath");
            }

            var fi = new FileInfo(filepath);
            var dotExt = fi.Extension.ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(dotExt))
            {
                throw new InvalidOperationException(string.Format("Unknown extension: {0}", dotExt));
            }

            if (!types.ContainsKey(dotExt))
            {
                var regPerm = new RegistryPermission(RegistryPermissionAccess.Read, "\\\\HKEY_CLASSES_ROOT");
                using (var classesRoot = Registry.ClassesRoot)
                using (var typeKey = classesRoot.OpenSubKey("MIME\\Database\\Content Type"))
                {
                    foreach (var keyname in typeKey.GetSubKeyNames())
                    {
                        using (var curKey = classesRoot.OpenSubKey("MIME\\Database\\Content Type\\" + keyname))
                        {
                            var extension = curKey.GetValue("Extension");
                            if (null != extension && extension.ToString().ToUpperInvariant() == dotExt)
                            {
                                types.Add(dotExt, keyname);
                                break;
                            }
                        }
                    }
                }
            }

            return types[dotExt];
        }
        #endregion
    }
}