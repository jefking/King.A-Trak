namespace King.ATrak.Windows
{
    using System.Collections.Generic;
    using System.IO;

    public class FolderWriter
    {
        protected readonly string to = null;

        public virtual void Initialize()
        {
            if (!Directory.Exists(this.to))
            {
                Directory.CreateDirectory(this.to);
            }
        }

        public virtual void Store(IEnumerable<IStorageItem> items)
        {

        }
    }
}