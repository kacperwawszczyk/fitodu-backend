using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Scheduledo.Core.Helpers
{
    public static class Language
    {
        private readonly static List<string> availableCultures = new List<string>
        {
            "en-GB",
            "pl-PL"
        };

        public static string DefaultCulture => availableCultures[0];
        public static string CurrentCulture { private set; get; }

        public static void SetLanguage(string language)
        {
            if (string.IsNullOrEmpty(language))
            {
                ChangeCurrentCulture(DefaultCulture);
                return;
            }

            language = language.ToLower();

            var culture = availableCultures.FirstOrDefault(
                x => x.ToLower().StartsWith(language, StringComparison.InvariantCulture));

            if (culture != null)
            {
                ChangeCurrentCulture(culture);
            }
            else
            {
                ChangeCurrentCulture(DefaultCulture);
            }
        }

        private static void ChangeCurrentCulture(string culture)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture(culture);
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            CurrentCulture = culture;
        }
    }
}