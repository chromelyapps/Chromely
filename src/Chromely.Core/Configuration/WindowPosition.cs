namespace Chromely.Core.Configuration
{
    public struct WindowPosition
    {
        public int X { get; }
        public int Y { get; }

        public WindowPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}