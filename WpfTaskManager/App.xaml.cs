using System.Windows;
using TaskManager.Controller;
using TaskManager.Localization;
using TaskManager.Model;
using TaskManager.View;
using WpfTaskManager.Themes;

namespace WpfTaskManager;

public partial class App : System.Windows.Application
{
    private ILocalizer _localizer;
    private IThemeManager _themeManager;
    protected override void OnStartup(StartupEventArgs e)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        base.OnStartup(e);
        _localizer = new ResourceLocalizer();
        _themeManager = new ThemeManager();
        var mainWindow = new MainWindow(_localizer, _themeManager);
        mainWindow.Show();
        //Dispatcher.Invoke(
        Run(mainWindow);
    }
    private async void Run(MainWindow mainWindow)
    {
        while (true)
        {
            ITaskRepository repository = new TaskRepository();

            var wpfView = new WpfView(_localizer, _themeManager);
            var controller = new TaskController(repository, wpfView, wpfView.WaitForKey);
            mainWindow.SetView(wpfView, controller);

            wpfView.Window = mainWindow;

            await Task.Run(controller.GetSelectionFromMenu);
        }
    }
}
