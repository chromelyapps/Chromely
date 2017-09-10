//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_thread_id_t.
//
namespace Xilium.CefGlue
{
    /// <summary>
    /// Existing thread IDs.
    /// </summary>
    public enum CefThreadId
    {
        // BROWSER PROCESS THREADS -- Only available in the browser process.

        /// <summary>
        /// The main thread in the browser. This will be the same as the main
        /// application thread if CefInitialize() is called with a
        /// CefSettings.multi_threaded_message_loop value of false.
        /// </summary>
        UI,

        /// <summary>
        /// Used to interact with the database.
        /// </summary>
        DB,

        /// <summary>
        /// Used to interact with the file system.
        /// </summary>
        File,

        /// <summary>
        /// Used for file system operations that block user interactions.
        /// Responsiveness of this thread affects users.
        /// </summary>
        FileUserBlocking,

        /// <summary>
        /// Used to launch and terminate browser processes.
        /// </summary>
        ProcessLauncher,

        /// <summary>
        /// Used to handle slow HTTP cache operations.
        /// </summary>
        Cache,

        /// <summary>
        /// Used to process IPC and network messages.
        /// </summary>
        IO,

        // RENDER PROCESS THREADS -- Only available in the render process.

        /// <summary>
        /// The main thread in the renderer. Used for all WebKit and V8 interaction.
        /// </summary>
        Renderer,
    }
}
