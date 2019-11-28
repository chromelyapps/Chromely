using System.Collections.Generic;
using System.Linq;

namespace Chromely.Core.Infrastructure
{
    public static class UrlSchemeCollectionExtension
    {
        public static bool IsUrlRegisteredExternalScheme(this List<UrlScheme> urlSchemes, string url)
        {
            if (urlSchemes == null ||
                !urlSchemes.Any() ||
                string.IsNullOrWhiteSpace(url))
                return false;

            return urlSchemes.Any((x => x.IsUrlOfSameScheme(url) && (x.UrlSchemeType == UrlSchemeType.External)));
        }

        public static bool IsUrlRegisteredCommandScheme(this List<UrlScheme> urlSchemes, string url)
        {
            if (urlSchemes == null ||
                 !urlSchemes.Any() ||
                 string.IsNullOrWhiteSpace(url))
                return false;

            return urlSchemes.Any((x => x.IsUrlOfSameScheme(url) && (x.UrlSchemeType == UrlSchemeType.Command)));
        }

        public static bool IsUrlRegisteredCustomScheme(this List<UrlScheme> urlSchemes, string url)
        {
            if (urlSchemes == null ||
                 !urlSchemes.Any() ||
                 string.IsNullOrWhiteSpace(url))
                return false;

            return urlSchemes.Any((x => x.IsUrlOfSameScheme(url) && (x.UrlSchemeType == UrlSchemeType.Custom)));
        }

        public static IEnumerable<UrlScheme> GetAllResouceSchemes(this List<UrlScheme> urlSchemes)
        {
            if (urlSchemes == null || !urlSchemes.Any())
                return new List<UrlScheme>();

            return urlSchemes.Where(x => x.UrlSchemeType == UrlSchemeType.Resource);
        }

        public static IEnumerable<UrlScheme> GetAllCustomSchemes(this List<UrlScheme> urlSchemes)
        {
            if (urlSchemes == null || !urlSchemes.Any())
                return new List<UrlScheme>();

            return urlSchemes.Where(x => x.UrlSchemeType == UrlSchemeType.Custom);
        }
    }
}
