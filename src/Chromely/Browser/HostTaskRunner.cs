using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chromely.Browser {

    /// <summary>
    ///     A Helper class for launching Chromely under a seperate thread / task, this helps with
    ///     cleanup on exit.
    /// </summary>
    public class HostTaskRunner {

        private Task BlazorTask;
        private CancellationTokenSource BlazorTaskTokenSource;
        private Action HostBuilderAction;
        private int Port;

        /// <summary> Constructor. </summary>
        /// <param name="action"> The action to run for creating the hostbuilder. </param>
        /// <param name="port">   The port to monitor. </param>
        public HostTaskRunner(Action action, int port) {
            HostBuilderAction = action;
            Port = port;
        }

        /// <summary> start the kestrel server in a background thread. </summary>
        public void Launch() {
            BlazorTaskTokenSource = new CancellationTokenSource();

            // start the kestrel server in a background thread.
            BlazorTask = new Task(HostBuilderAction, BlazorTaskTokenSource.Token, TaskCreationOptions.LongRunning);
            BlazorTask.Start();

            // wait untill its up
            while (ClientUrlHelper.IsPortAvailable(Port)) {
                Thread.Sleep(1);
            }
        }

        /// <summary>
        ///     Clean up kestrel process if not taken down by OS. This can occur when the app is closed
        ///     from WindowController (frameless).
        /// </summary>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        public void ProcessExit(object sender, EventArgs e) {
            Task.Run(() => {
                WaitHandle.WaitAny(new[] { BlazorTaskTokenSource?.Token.WaitHandle });
            });
            BlazorTaskTokenSource?.Cancel();
        }
    }
}
