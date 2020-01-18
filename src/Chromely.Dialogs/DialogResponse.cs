namespace Chromely.Dialogs
{
    public class DialogResponse
    {
        public bool IsCanceled { get; set; }
        public object Value { get; set; }

        public DialogResponse()
        {
            IsCanceled = false;
        }
    }
}
