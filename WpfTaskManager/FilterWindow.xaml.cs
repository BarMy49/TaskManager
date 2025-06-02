using System.Windows;
using System.Windows.Controls;
using TaskManager.Controller;
using TaskManager.Localization;
using TaskManager.View;
using WpfTaskManager.Themes;
using ComboBox =  System.Windows.Controls.ComboBox;

namespace WpfTaskManager;

public partial class FilterWindow : Window
{
    private readonly WpfView _view;
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
        this.Title = localizer.GetString("FilterByText");
        
        InitializeComponent();
    }
    
    private void FilterTypeChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox)
        {
            _view.CategoryFilterVisibility = comboBox.SelectedIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
            _view.DateFromFilterVisibility = comboBox.SelectedIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
            _view.DateToFilterVisibility = comboBox.SelectedIndex == 2 ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    private void FilterCategory(object sender, RoutedEventArgs e)
    {
        _view.Ignore = true;
        _controller.FilterTasks(0);
        this.Close();
    }
    private void FilterDateFrom(object sender, RoutedEventArgs e)
    {
        _view.Ignore = true;
        _controller.FilterTasks(2);
        this.Close();
    }
    private void FilterDateTo(object sender, RoutedEventArgs e)
    {
        _view.Ignore = true;
        _controller.FilterTasks(1);
        this.Close();
    }
    
}