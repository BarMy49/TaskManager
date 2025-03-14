using System.Windows;
using TaskManager.Controller;
using TaskManager.Model;
using TaskManager.View;

namespace WpfTaskManager;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var mainWindow = new MainWindow();
        mainWindow.Show();
        //Dispatcher.Invoke(
        Run(mainWindow);
    }
    private async void Run(MainWindow mainWindow)
    {
        while (true)
        {
            ITaskRepository repository = new TaskRepository();

            var wpfView = new WpfView();
            var controller = new TaskController(repository, wpfView, wpfView.WaitForKey);
            mainWindow.SetView(wpfView, controller);

            wpfView.Window = mainWindow;

            await Task.Run(controller.GetSelectionFromMenu);
        }
    }
}
