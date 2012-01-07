namespace Abc.ATrak
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Permissions;
    using Microsoft.Win32;

    /// <summary>
    /// Content Types
    /// </summary>
    public static class ContentTypes
    {
        #region Members
        /// <summary>
        /// Content Types
        /// </summary>
        private static IDictionary<string, string> types = new Dictionary<string, string>();
        #endregion

        #region Methods
        /// <summary>
        /// Content Type
        /// </summary>
        /// <param name="filepath">File Path</param>
        /// <returns>Content Type</returns>
        public static string ContentType(string filepath)
        {
            if (string.IsNullOrWhiteSpace(filepath))
            {
                throw new ArgumentException("filepath");
            }
            else
            {
                var fi = new FileInfo(filepath);
                var dotExt = fi.Extension.ToLowerInvariant();

                if (string.IsNullOrWhiteSpace(filepath))
                {
                    throw new InvalidOperationException(string.Format("Unknown extension; file: {0}", filepath));
                }
                else if (types.ContainsKey(dotExt))
                {
                    return types[dotExt];
                }
                else
                {
                    var regPerm = new RegistryPermission(RegistryPermissionAccess.Read, "\\\\HKEY_CLASSES_ROOT");
                    using (var classesRoot = Registry.ClassesRoot)
                    {
                        using (var typeKey = classesRoot.OpenSubKey("MIME\\Database\\Content Type"))
                        {
                            foreach (var keyname in typeKey.GetSubKeyNames())
                            {
                                using (var curKey = classesRoot.OpenSubKey("MIME\\Database\\Content Type\\" + keyname))
                                {
                                    var extension = curKey.GetValue("Extension");
                                    if (null != extension && extension.ToString().ToLowerInvariant() == dotExt)
                                    {
                                        return keyname;
                                    }
                                }
                            }
                        }
                    }

                    return null;
                }
            }
        }
        #endregion
    }
}