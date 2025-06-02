using System.Windows;
using TaskManager.Controller;
using TaskManager.Localization;
using TaskManager.View;
using WpfTaskManager.Themes;

namespace WpfTaskManager;

public partial class AuxiliaryWindow : Window
{
    private readonly WpfView _view;
    private readonly string _mode;
    private readonly ILocalizer _localizer;
    private readonly TaskController _controller;
    private readonly IThemeManager _themeManager;
    public AuxiliaryWindow(string mode, ILocalizer localizer, TaskController controller, IThemeManager themeManager, WpfView view)
    {
        _localizer = localizer;
        _mode = mode;
        _controller = controller;
        _themeManager = themeManager;
        _view = view;
        this.DataContext = view;
        
        if (_mode == "A")
        {
            this.Title = _localizer.GetString("AddTask");
            view.AddConfirmVisibility = Visibility.Visible;
            view.EditConfirmVisibility = Visibility.Collapsed;
        }
        else if (_mode == "E")
        {
            _controller.PopulateFields();
            this.Title = _localizer.GetString("EditTask");
            view.AddConfirmVisibility = Visibility.Collapsed;
            view.EditConfirmVisibility = Visibility.Visible;
        }
        
        InitializeComponent();
    }


    private void AddTask(object sender, RoutedEventArgs e)
    {
        if (_view != null && _controller != null)
        {
            _view.Ignore = true;
            _controller.AddTask();
        }
        _controller.Notification($"{_localizer.GetString("AddSuccess")}", "");
        _view.ClearTaskFields();
    }

    private void EditTask(object sender, RoutedEventArgs e)
    {
        if (_view != null && _controller != null)
        {
            _view.Ignore = true;
            _controller.EditTask();
        }
        _controller.Notification($"{_localizer.GetString("EditSuccess")}{_view.EditId}.", "");
        this.Close();
    }
}