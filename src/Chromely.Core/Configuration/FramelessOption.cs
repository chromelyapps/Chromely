namespace Chromely.Core.Configuration
{
    public class FramelessOption
    {
        public FramelessOption()
        {
            IsDraggable = true;
            UseWebkitAppRegions = false;
            DraggableHeight = 32;
            NonDraggableRightOffsetWidth = 140;
        }

        public int DraggableHeight { get; set; }
        public int NonDraggableRightOffsetWidth { get; set; }
        public bool IsDraggable { get; set; }
        public bool UseWebkitAppRegions { get; set; }
    }
}