using System.Globalization;

namespace TaskManager.Localization
{
    public interface ILocalizer
    {
        string GetString(string key);
        void SetLanguage(string language);
        string CurrentLanguage { get; }
        CultureInfo CurrentCulture { get; }
    }
}