namespace Chromely.Core.Host
{
    public class WindowCustomStyle
    {
        public WindowCustomStyle(int styles, int exStyles)
        {
            WindowStyles = styles;
            WindowExStyles = exStyles;
        }

        public int WindowStyles { get; set; }
        public int WindowExStyles { get; set; }

        public bool IsValid()
        {
            return ((WindowStyles != 0) && (WindowExStyles != 0));
        }
    }
}
