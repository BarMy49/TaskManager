using System.Windows;
using TaskManager.Controller;
using TaskManager.Localization;
using TaskManager.View;
using WpfTaskManager.Themes;

namespace WpfTaskManager;

public partial class FilterWindow : Window
{
    private readonly WpfView _view;
    private readonly string _mode;
    private readonly ILocalizer _localizer;
    private readonly TaskController _controller;
    private readonly IThemeManager _themeManager;
    public FilterWindow(ILocalizer localizer, TaskController controller, IThemeManager themeManager, WpfView view)
    {
        _localizer = localizer;
        _controller = controller;
        _themeManager = themeManager;
        _view = view;
        this.DataContext = view;
        
        controller.UpdateCategoryFilters();
        this.Title = localizer.GetString("FilterByCategory");
        
        InitializeComponent();
    }

    private void Filter(object sender, RoutedEventArgs e)
    {
        _view.Ignore = true;
        _controller.FilterTasks(0);
        this.Close();
    }
}