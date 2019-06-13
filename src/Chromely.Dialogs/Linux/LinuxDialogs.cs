using System;
using System.IO;
using Chromely.Core.Infrastructure;

namespace Chromely.Dialogs.Linux
{
    public class LinuxDialogs : IChromelyDialogs
    {
        public LinuxDialogs()
        {
            GtkInterop.gtk_init(0, new string[0]);
        }
        
        public DialogResponse MessageBox(string message, DialogOptions options)
        {
            const GtkInterop.DialogFlags flags = GtkInterop.DialogFlags.GTK_DIALOG_MODAL;

            var type = GtkInterop.MessageType.GTK_MESSAGE_INFO;
            switch (options.Icon)
            {
                case DialogIcon.None:
                    break;
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

            const GtkInterop.ButtonsType bt = GtkInterop.ButtonsType.GTK_BUTTONS_OK;
            
            var title = GtkInterop.AllocGtkString(options.Title);
            var widget = IntPtr.Zero;
            var response = GtkInterop.ResponseType.GTK_RESPONSE_OK;
            try
            {
                widget = GtkInterop.gtk_message_dialog_new(IntPtr.Zero, flags, type, bt, title, IntPtr.Zero);
                GtkInterop.gtk_message_dialog_format_secondary_text(widget, message);
                response = GtkInterop.gtk_dialog_run(widget);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                GtkInterop.gtk_widget_destroy(widget);
                GtkInterop.g_free(title);
            }

            return new DialogResponse { IsCanceled = response == GtkInterop.ResponseType.GTK_RESPONSE_DELETE_EVENT };
        }

        public DialogResponse SelectFolder(string message, FileDialogOptions options)
        {
            var widget = IntPtr.Zero;
            var response = GtkInterop.ResponseType.GTK_RESPONSE_OK;
            var ok = GtkInterop.AllocGtkString("OK");
            var cancel = GtkInterop.AllocGtkString("Cancel");
            var folder = "";
            try
            {
                widget = GtkInterop.gtk_file_chooser_dialog_new(options.Title, IntPtr.Zero,
                    GtkInterop.FileChooserAction.GTK_FILE_CHOOSER_ACTION_SELECT_FOLDER, 
                    ok, (int)GtkInterop.ResponseType.GTK_RESPONSE_OK, 
                    cancel, (int)GtkInterop.ResponseType.GTK_RESPONSE_CANCEL,
                    IntPtr.Zero);
                response = GtkInterop.gtk_dialog_run(widget);
                if(response == GtkInterop.ResponseType.GTK_RESPONSE_OK)
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
            
            return new DialogResponse
            {
                IsCanceled = response != GtkInterop.ResponseType.GTK_RESPONSE_OK, 
                Value = folder
            };
        }

        public DialogResponse FileOpen(string message, FileDialogOptions options)
        {
            var widget = IntPtr.Zero;
            var response = GtkInterop.ResponseType.GTK_RESPONSE_OK;
            var ok = GtkInterop.AllocGtkString("OK");
            var cancel = GtkInterop.AllocGtkString("Cancel");
            var fileName = "";
            try
            {
                widget = GtkInterop.gtk_file_chooser_dialog_new(options.Title, IntPtr.Zero,
                    GtkInterop.FileChooserAction.GTK_FILE_CHOOSER_ACTION_OPEN, 
                    ok, (int)GtkInterop.ResponseType.GTK_RESPONSE_OK, 
                    cancel, (int)GtkInterop.ResponseType.GTK_RESPONSE_CANCEL,
                    IntPtr.Zero);
                
                if (!string.IsNullOrEmpty(options.Directory) && Directory.Exists(options.Directory))
                {
                    GtkInterop.gtk_file_chooser_set_current_folder(widget, options.Directory);
                }
                if (options.MustExist)
                {
                    Log.Error("FileOpen option MustExist not supported.");
                }

                foreach (var fileFilter in options.Filters)
                {
                    var filter = GtkInterop.gtk_file_filter_new();
                    GtkInterop.gtk_file_filter_set_name(filter, fileFilter.Name);
                    GtkInterop.gtk_file_filter_add_pattern(filter, $"*.{fileFilter.Extension}");
                    GtkInterop.gtk_file_chooser_add_filter(widget, filter);
                }
                
                response = GtkInterop.gtk_dialog_run(widget);
                if(response == GtkInterop.ResponseType.GTK_RESPONSE_OK)
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
            
            return new DialogResponse
            {
                IsCanceled = response != GtkInterop.ResponseType.GTK_RESPONSE_OK, 
                Value = fileName
            };
        }

        public DialogResponse FileSave(string message, string fileName, FileDialogOptions options)
        {
            var widget = IntPtr.Zero;
            var response = GtkInterop.ResponseType.GTK_RESPONSE_OK;
            var ok = GtkInterop.AllocGtkString("OK");
            var cancel = GtkInterop.AllocGtkString("Cancel");
            
            try
            {
                widget = GtkInterop.gtk_file_chooser_dialog_new(options.Title, IntPtr.Zero,
                    GtkInterop.FileChooserAction.GTK_FILE_CHOOSER_ACTION_SAVE, 
                    ok, (int)GtkInterop.ResponseType.GTK_RESPONSE_OK, 
                    cancel, (int)GtkInterop.ResponseType.GTK_RESPONSE_CANCEL,
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
                if(response == GtkInterop.ResponseType.GTK_RESPONSE_OK)
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
            
            return new DialogResponse
            {
                IsCanceled = response != GtkInterop.ResponseType.GTK_RESPONSE_OK, 
                Value = fileName
            };
        }
    }
}