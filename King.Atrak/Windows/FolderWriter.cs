namespace King.ATrak.Windows
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
    }
}