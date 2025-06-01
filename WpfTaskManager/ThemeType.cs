using System.ComponentModel;
using System.Windows;

namespace WpfTaskManager.Themes
{
    public enum ThemeType
    {
        Light,
        Dark,
        Task,
        Angular,
        Funky
    }
    
    public interface IThemeManager
    {
        ThemeType CurrentTheme { get; }
        void SetTheme(ThemeType theme);
    }
    
    public class ThemeManager : IThemeManager, INotifyPropertyChanged
    {
        private ThemeType _currentTheme = ThemeType.Task;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public ThemeType CurrentTheme => _currentTheme;
        
        public void SetTheme(ThemeType theme)
        {
            _currentTheme = theme;
            ApplyTheme();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentTheme)));
        }
        
        private void ApplyTheme()
        {
            var app = System.Windows.Application.Current;
            if (app == null) return;
            
            ResourceDictionary resourceDict = new ResourceDictionary();
            resourceDict.Source = new Uri($"/WpfTaskManager;component/Themes/{_currentTheme}Theme.xaml", UriKind.Relative);
            
            app.Resources.MergedDictionaries.Clear();
            app.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}