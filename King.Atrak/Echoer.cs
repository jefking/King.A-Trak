namespace King.ATrak
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Echo deletions to destination
    /// </summary>
    public class Echoer : IEchoer
    {
        #region Members
        /// <summary>
        /// Destination
        /// </summary>
        protected readonly IDataLister destination = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public Echoer(IDataLister destination)
        {
            if (null == destination)
            {
                throw new ArgumentNullException("destination");
            }

            this.destination = destination;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Clean Destination
        /// </summary>
        /// <param name="sourceItems">Source Items</param>
        public virtual async Task CleanDestination(IEnumerable<IStorageItem> sourceItems)
        {
            if (null == sourceItems)
            {
                throw new ArgumentNullException("sourceItems");
            }

            if (sourceItems.Any())
            {
                var destinationItems = this.destination.List();
                if (null != destinationItems && destinationItems.Any())
                {
                    var notFoundItems = destinationItems.Where(di => !sourceItems.Any(si => si.RelativePath == di.RelativePath));
                    foreach (var item in notFoundItems)
                    {
                        Trace.TraceInformation("Deleting item in destination: '{0}'.", item.RelativePath);

                        await item.Delete();
                    }
                }
            }
        }
        #endregion
    }
}