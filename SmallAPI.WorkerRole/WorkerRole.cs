using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure.ServiceRuntime;
using SmallApi.WorkerRole.Configuration;

namespace SmallApi.WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private IDisposable app;

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole1 is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            var endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Endpoint1"];
            string baseUri = $"{endpoint.Protocol}://{endpoint.IPEndpoint}";

            Trace.TraceInformation($"Starting OWIN at {baseUri}",
                "Information");

            app = WebApp.Start<Startup>(new StartOptions(url: baseUri));
            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            app?.Dispose();
            base.OnStop();

            Trace.TraceInformation("WorkerRole1 has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
