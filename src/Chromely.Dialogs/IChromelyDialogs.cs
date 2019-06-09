namespace Chromely.Dialogs
{
    public interface IChromelyDialogs
    {
        DialogResponse MessageBox(string message, DialogOptions options);
        
        DialogResponse SelectFolder(string message, FileDialogOptions options);
        DialogResponse FileOpen(string message, FileDialogOptions options);
        DialogResponse FileSave(string message, string fileName, FileDialogOptions options);
        
    }
}