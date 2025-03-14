using System;
using TaskManager.Model;
using TaskManager.View;
using TaskManager.Controller;
using System.Diagnostics;

namespace TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            { // Inicjalizacja Modelu
                ITaskRepository repository = new TaskRepository();

                // Inicjalizacja Widoku
                ConsoleView view = new ConsoleView();


                // Inicjalizacja Kontrolera
                TaskController controller = new TaskController(repository, view, () => Console.ReadKey(true).Key);

                // Uruchomienie aplikacji
                controller.GetSelectionFromMenu();

            } while (true);
        }
    }
}
