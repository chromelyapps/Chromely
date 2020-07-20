// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Chromely.Core
{
    public class ChromiumBuildNumber
    {
        public int Major { get; }
        public int Minor { get; }
        public int Build { get; }
        public int Patch { get; }

        public ChromiumBuildNumber(int major, int minor, int build, int patch)
        {
            Major = major;
            Minor = minor;
            Build = build;
            Patch = patch;
        }

        public static ChromiumBuildNumber Parse(string version)
        {
            if (string.IsNullOrEmpty(version)) return null;
            
            var parts = version.Split('.');
            if(parts.Length != 4) return null;
            
            int.TryParse(parts[0], out var major);
            int.TryParse(parts[1], out var minor);
            int.TryParse(parts[2], out var build);
            int.TryParse(parts[3], out var patch);
            
            return new ChromiumBuildNumber(major, minor, build, patch);
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Build}.{Patch}";
        }
    }
}