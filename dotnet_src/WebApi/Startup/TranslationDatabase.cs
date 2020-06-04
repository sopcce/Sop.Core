using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi
{
    /// <summary>
    /// </summary>
    public class TranslationDatabase
    {
        private static readonly Dictionary<string, Dictionary<string, string>> Translations =
            new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "en-us", new Dictionary<string, string>
                    {
                        {"WeatherForecast", "WeatherForecast"},
                        {"list", "Index"}
                    }
                },
                {
                    "zh-cn", new Dictionary<string, string>
                    {
                        {"WeatherForecast", "Get"},
                        {"liste", "Index"}
                    }
                },
                {
                    "ja-jp", new Dictionary<string, string>
                    {
                        {"WeatherForecast", "Get"},
                        {"lista", "Index"}
                    }
                }
            };

        /// <summary>
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<string> Resolve(string lang, string value)
        {
            var normalizedLang = lang.ToLowerInvariant();
            var normalizedValue = value.ToLowerInvariant();
            if (Translations.ContainsKey(normalizedLang) && Translations[normalizedLang].ContainsKey(normalizedValue))
                return Translations[normalizedLang][normalizedValue];
            return await Task.FromResult("");
        }
    }
}