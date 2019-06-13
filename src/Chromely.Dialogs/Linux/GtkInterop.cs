using System;
using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable ArrangeTypeMemberModifiers

namespace Chromely.Dialogs.Linux
{
    internal static class GtkInterop
    {
        internal const string GtkLib = "libgtk-x11-2.0.so.0";
        
        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
        internal static extern void gtk_init(int argc, string[] argv);

        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern IntPtr g_malloc(UIntPtr n_bytes);


        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern void g_free(IntPtr ptr);

        public static IntPtr AllocGtkString(string str)
        {
            if (str == null)
                return IntPtr.Zero;
            var bytes = System.Text.Encoding.UTF8.GetBytes(str);
            var result = g_malloc(new UIntPtr((ulong) bytes.Length + 1));
            Marshal.Copy(bytes, 0, result, bytes.Length);
            Marshal.WriteByte(result, bytes.Length, 0);
            return result;
        }


        [Flags]
        internal enum DialogFlags
        {
            GTK_DIALOG_MODAL = 0,
            GTK_DIALOG_DESTROY_WITH_PARENT = 1,
            GTK_DIALOG_USE_HEADER_BAR = 2
        }

        internal enum MessageType
        {
            GTK_MESSAGE_INFO,
            GTK_MESSAGE_WARNING,
            GTK_MESSAGE_QUESTION,
            GTK_MESSAGE_ERROR,
            GTK_MESSAGE_OTHER
        }

        internal enum ButtonsType
        {
            GTK_BUTTONS_NONE,
            GTK_BUTTONS_OK,
            GTK_BUTTONS_CLOSE,
            GTK_BUTTONS_CANCEL,
            GTK_BUTTONS_YES_NO,
            GTK_BUTTONS_OK_CANCEL
        }

        internal enum ResponseType
        {
            GTK_RESPONSE_NONE = -1,
            GTK_RESPONSE_REJECT = -2,
            GTK_RESPONSE_ACCEPT = -3,
            GTK_RESPONSE_DELETE_EVENT = -4,
            GTK_RESPONSE_OK = -5,
            GTK_RESPONSE_CANCEL = -6,
            GTK_RESPONSE_CLOSE = -7,
            GTK_RESPONSE_YES = -8,
            GTK_RESPONSE_NO = -9,
            GTK_RESPONSE_APPLY = -10,
            GTK_RESPONSE_HELP = -11
        }


        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern IntPtr gtk_message_dialog_new(IntPtr parent_window, DialogFlags flags, MessageType type,
            ButtonsType bt, IntPtr msg, IntPtr args);

        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern void gtk_message_dialog_format_secondary_text(IntPtr message_dialog, string message);


        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern void gtk_widget_show_all(IntPtr widget);

        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern ResponseType gtk_dialog_run(IntPtr dialog);

        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern void gtk_widget_destroy(IntPtr widget);


        internal enum FileChooserAction
        {
        GTK_FILE_CHOOSER_ACTION_OPEN,
        GTK_FILE_CHOOSER_ACTION_SAVE,
        GTK_FILE_CHOOSER_ACTION_SELECT_FOLDER,
        GTK_FILE_CHOOSER_ACTION_CREATE_FOLDER
        }
        
        
        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern IntPtr gtk_file_chooser_dialog_new(string title, IntPtr parent, FileChooserAction action, 
            IntPtr first_button_text, int first_button_id, IntPtr second_button_text, int second_button_id, IntPtr args);


        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern void gtk_file_chooser_set_current_folder(IntPtr chooser, string fileName);
        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern void gtk_file_chooser_set_filename(IntPtr chooser, string fileName);
        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern void gtk_file_chooser_set_current_name(IntPtr chooser, string fileName);
        
        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern string gtk_file_chooser_get_uri(IntPtr chooser);
        
        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern string gtk_file_chooser_get_filename(IntPtr chooser);
        
        
        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern void gtk_file_chooser_set_do_overwrite_confirmation(IntPtr chooser, bool value);
        
        
        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        
        internal static extern void gtk_file_chooser_add_filter(IntPtr chooser, IntPtr filter);
        
        
        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern IntPtr gtk_file_filter_new();
        
        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern void gtk_file_filter_set_name(IntPtr filter, string name);
        
        [DllImport(GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern void gtk_file_filter_add_pattern(IntPtr filter, string pattern);
    }
}