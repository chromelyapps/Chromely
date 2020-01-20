using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Chromely.CefGlue.Browser;
using Chromely.Core.Helpers;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Xilium.CefGlue;

namespace Chromely.Dialogs.Linux
{
    public class LinuxDialogs : IChromelyDialogs
    {
        private bool isInitialized;
        private IChromelyWindow _window;
        private ExecutionContext _executionContext;

        //private IntPtr MainWindowHandle => _window?.Handle ?? IntPtr.Zero;
        private IntPtr MainWindowHandle => IntPtr.Zero;
        
        public void Init(IChromelyWindow window)
        {
            if (isInitialized) return;
            
            _window = window;

            //GtkInterop.XInitThreads();
            //GtkInterop.gtk_init(0, new string[0]);
            //GtkInterop.g_type_init();

            _executionContext = Thread.CurrentThread.ExecutionContext.CreateCopy();
            isInitialized = true;
        }

        public DialogResponse MessageBox(string message, DialogOptions options)
        {
            var result = new DialogResponse();
            _executionContext.InvokeAsyncIfPossible(() => { result = MessageBoxIntl(message, options); });
            return result;
        }

        public DialogResponse MessageBoxIntl(string message, DialogOptions options)
        {
            Console.WriteLine($"MessageBox PID={Process.GetCurrentProcess().Id}, THREAD={Thread.CurrentThread.ManagedThreadId}");
            
            GtkInterop.gdk_threads_enter();

            const GtkInterop.DialogFlags flags = GtkInterop.DialogFlags.GTK_DIALOG_DESTROY_WITH_PARENT;

            var type = GtkInterop.MessageType.GTK_MESSAGE_NONE;
            switch (options.Icon)
            {
                case DialogIcon.Question:
                    type = GtkInterop.MessageType.GTK_MESSAGE_QUESTION;
                    break;
                case DialogIcon.Information:
                    type = GtkInterop.MessageType.GTK_MESSAGE_INFO;
                    break;
                case DialogIcon.Warning:
                    type = GtkInterop.MessageType.GTK_MESSAGE_WARNING;
                    break;
                case DialogIcon.Error:
                    type = GtkInterop.MessageType.GTK_MESSAGE_ERROR;
                    break;
            }

            const GtkInterop.ButtonsType button = GtkInterop.ButtonsType.GTK_BUTTONS_OK;

            var title = GtkInterop.AllocGtkString(options.Title);
            var widget = IntPtr.Zero;
            var response = GtkInterop.ResponseType.GTK_RESPONSE_OK;
            try
            {
                Console.WriteLine("#1");
                widget = GtkInterop.gtk_message_dialog_new(MainWindowHandle, flags, type, button, IntPtr.Zero, IntPtr.Zero);
                Console.WriteLine("#2");
                GtkInterop.gtk_message_dialog_format_secondary_text(widget, message);
                Console.WriteLine("#3");
                response = GtkInterop.gtk_dialog_run(widget);
                Console.WriteLine("#4");
            }
            catch (Exception ex)
            {
                Console.WriteLine("#5");
                Console.WriteLine("MessageBox: " + ex.Message);
            }
            finally
            {
                GtkInterop.gtk_widget_destroy(widget);
                GtkInterop.g_free(title);
            }

            GtkInterop.gdk_threads_leave();

            return new DialogResponse {IsCanceled = response == GtkInterop.ResponseType.GTK_RESPONSE_DELETE_EVENT};
        }

        public DialogResponse SelectFolder(string message, FileDialogOptions options)
        {
            GtkInterop.gdk_threads_enter();

            Console.WriteLine("#1");

            var widget = IntPtr.Zero;
            var response = GtkInterop.ResponseType.GTK_RESPONSE_OK;
            var ok = GtkInterop.AllocGtkString("OK");
            var cancel = GtkInterop.AllocGtkString("Cancel");
            var folder = "";
            try
            {
                Console.WriteLine($"#2 MainWindowHandle={MainWindowHandle}");
                widget = GtkInterop.gtk_file_chooser_dialog_new(
                    options.Title,
                    MainWindowHandle, 
                    GtkInterop.FileChooserAction.GTK_FILE_CHOOSER_ACTION_SELECT_FOLDER,
                    ok, (int) GtkInterop.ResponseType.GTK_RESPONSE_OK,
                    cancel, (int) GtkInterop.ResponseType.GTK_RESPONSE_CANCEL,
                    IntPtr.Zero);
                Console.WriteLine("#3");

                Console.WriteLine("#4");
                response = GtkInterop.gtk_dialog_run(widget);
                Console.WriteLine("#5");
                if (response == GtkInterop.ResponseType.GTK_RESPONSE_OK)
                {
                    var uri = new Uri(GtkInterop.gtk_file_chooser_get_uri(widget));
                    folder = Uri.UnescapeDataString(uri.AbsolutePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                GtkInterop.gtk_widget_destroy(widget);
                GtkInterop.g_free(ok);
                GtkInterop.g_free(cancel);
            }

            GtkInterop.gdk_threads_leave();

            return new DialogResponse
            {
                IsCanceled = response != GtkInterop.ResponseType.GTK_RESPONSE_OK,
                Value = folder
            };
        }

        public DialogResponse FileOpen(string message, FileDialogOptions options)
        {
            GtkInterop.gdk_threads_enter();

            var widget = IntPtr.Zero;
            var response = GtkInterop.ResponseType.GTK_RESPONSE_OK;
            var ok = GtkInterop.AllocGtkString("OK");
            var cancel = GtkInterop.AllocGtkString("Cancel");
            var fileName = "";
            try
            {
                Console.WriteLine("#1");
                widget = GtkInterop.gtk_file_chooser_dialog_new(
                    options.Title, 
                    MainWindowHandle,
                    GtkInterop.FileChooserAction.GTK_FILE_CHOOSER_ACTION_OPEN,
                    ok, (int) GtkInterop.ResponseType.GTK_RESPONSE_OK,
                    cancel, (int) GtkInterop.ResponseType.GTK_RESPONSE_CANCEL,
                    IntPtr.Zero);

                if (!string.IsNullOrEmpty(options.Directory) && Directory.Exists(options.Directory))
                {
                    GtkInterop.gtk_file_chooser_set_current_folder(widget, options.Directory);
                }

                if (options.MustExist)
                {
                    Trace.TraceError("FileOpen option MustExist not supported.");
                }

                foreach (var fileFilter in options.Filters)
                {
                    var filter = GtkInterop.gtk_file_filter_new();
                    GtkInterop.gtk_file_filter_set_name(filter, fileFilter.Name);
                    GtkInterop.gtk_file_filter_add_pattern(filter, $"*.{fileFilter.Extension}");
                    GtkInterop.gtk_file_chooser_add_filter(widget, filter);
                }

                Console.WriteLine("#2");
                response = GtkInterop.gtk_dialog_run(widget);
                if (response == GtkInterop.ResponseType.GTK_RESPONSE_OK)
                {
                    fileName = Uri.UnescapeDataString(GtkInterop.gtk_file_chooser_get_filename(widget));
                }
                Console.WriteLine("#3");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                GtkInterop.gtk_widget_destroy(widget);
                GtkInterop.g_free(ok);
                GtkInterop.g_free(cancel);
            }

            GtkInterop.gdk_threads_leave();

            return new DialogResponse
            {
                IsCanceled = response != GtkInterop.ResponseType.GTK_RESPONSE_OK,
                Value = fileName
            };
        }

        public DialogResponse FileSave(string message, string fileName, FileDialogOptions options)
        {
            GtkInterop.gdk_threads_enter();

            var widget = IntPtr.Zero;
            var response = GtkInterop.ResponseType.GTK_RESPONSE_OK;
            var ok = GtkInterop.AllocGtkString("OK");
            var cancel = GtkInterop.AllocGtkString("Cancel");

            try
            {
                widget = GtkInterop.gtk_file_chooser_dialog_new(
                    options.Title,
                    MainWindowHandle,
                    GtkInterop.FileChooserAction.GTK_FILE_CHOOSER_ACTION_SAVE,
                    ok, (int) GtkInterop.ResponseType.GTK_RESPONSE_OK,
                    cancel, (int) GtkInterop.ResponseType.GTK_RESPONSE_CANCEL,
                    IntPtr.Zero);

                if (options.ConfirmOverwrite)
                {
                    GtkInterop.gtk_file_chooser_set_do_overwrite_confirmation(widget, true);
                }

                if (!string.IsNullOrEmpty(options.Directory) && Directory.Exists(options.Directory))
                {
                    GtkInterop.gtk_file_chooser_set_current_folder(widget, options.Directory);
                    GtkInterop.gtk_file_chooser_set_current_name(widget, fileName);
                }
                else
                {
                    GtkInterop.gtk_file_chooser_set_filename(widget, fileName);
                }

                foreach (var fileFilter in options.Filters)
                {
                    var filter = GtkInterop.gtk_file_filter_new();
                    GtkInterop.gtk_file_filter_set_name(filter, fileFilter.Name);
                    GtkInterop.gtk_file_filter_add_pattern(filter, $"*.{fileFilter.Extension}");
                    GtkInterop.gtk_file_chooser_add_filter(widget, filter);
                }

                response = GtkInterop.gtk_dialog_run(widget);
                if (response == GtkInterop.ResponseType.GTK_RESPONSE_OK)
                {
                    fileName = Uri.UnescapeDataString(GtkInterop.gtk_file_chooser_get_filename(widget));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                GtkInterop.gtk_widget_destroy(widget);
                GtkInterop.g_free(ok);
                GtkInterop.g_free(cancel);
            }

            GtkInterop.gdk_threads_leave();

            return new DialogResponse
            {
                IsCanceled = response != GtkInterop.ResponseType.GTK_RESPONSE_OK,
                Value = fileName
            };
        }
    }
}
