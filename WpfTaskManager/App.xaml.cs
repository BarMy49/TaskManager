using System.Windows;
using TaskManager.Controller;
using TaskManager.Localization;
using TaskManager.Model;
using TaskManager.View;

namespace WpfTaskManager;

public partial class App : Application
{
    private ILocalizer _localizer;
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        _localizer = new ResourceLocalizer();
        var mainWindow = new MainWindow(_localizer);
        mainWindow.Show();
        //Dispatcher.Invoke(
        Run(mainWindow);
    }
    private async void Run(MainWindow mainWindow)
    {
        while (true)
        {
            ITaskRepository repository = new TaskRepository();

            var wpfView = new WpfView(_localizer);
            var controller = new TaskController(repository, wpfView, wpfView.WaitForKey);
            mainWindow.SetView(wpfView, controller);

            wpfView.Window = mainWindow;

            await Task.Run(controller.GetSelectionFromMenu);
        }
    }
}
