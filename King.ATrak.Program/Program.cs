namespace King.ATrak
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// A-Trak synchronizer, between Azure and Windows
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
            Trace.TraceInformation("Starting...");

            try
            {
                var parameters = new Parameters(args);
                var config = parameters.Process();

                Trace.TraceInformation("{2}From: '{0}'{2}To: '{1}'{2}"
                    , string.Format("{0}", config.Source.Folder ?? config.Source.ContainerName)
                    , string.Format("{0}", config.Destination.Folder ?? config.Destination.ContainerName)
                    , Environment.NewLine);

                var sync = new Synchronizer(config);
                sync.Run().Wait();
            }
            catch (Exception ex)
            {
                Trace.Fail(ex.ToString());
            }

            Trace.TraceInformation("Completed.");

            Thread.Sleep(2000);
        }
        #endregion
    }
}