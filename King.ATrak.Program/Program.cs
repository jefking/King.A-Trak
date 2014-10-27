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

                Trace.TraceInformation("{3}Connection String: '{0}'{3}Container: '{1}'{3}Folder: '{2}'{3}"
                    , config.ConnectionString
                    , config.ContainerName
                    , config.Folder
                    , Environment.NewLine);

                var sync = new Synchronizer(config);
                sync.Run(config.SyncDirection).Wait();
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