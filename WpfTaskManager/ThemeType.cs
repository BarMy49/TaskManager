using System.ComponentModel;
using System.Windows;

namespace WpfTaskManager.Themes
{
    public enum ThemeType
    {
        Light,
        Dark
    }
    
    public interface IThemeManager
    {
        ThemeType CurrentTheme { get; }
        void SetTheme(ThemeType theme);
    }
    
    public class ThemeManager : IThemeManager, INotifyPropertyChanged
    {
        private ThemeType _currentTheme = ThemeType.Light;
        
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
            var app = Application.Current;
            if (app == null) return;
            
            ResourceDictionary resourceDict = new ResourceDictionary();
            
            if (_currentTheme == ThemeType.Dark)
            {
                resourceDict.Source = new Uri("/WpfTaskManager;component/Themes/DarkTheme.xaml", UriKind.Relative);
            }
            else
            {
                resourceDict.Source = new Uri("/WpfTaskManager;component/Themes/LightTheme.xaml", UriKind.Relative);
            }
            
            app.Resources.MergedDictionaries.Clear();
            app.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}