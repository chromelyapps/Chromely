namespace Chromely.Core.Configuration
{
    public struct WindowSize
    {
        public int Width { get; }
        public int Height { get; }

        public WindowSize( int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}