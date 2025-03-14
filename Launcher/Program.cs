using System.Diagnostics;

string wpf = @"C:\S. KCK\Graficzny Poprawa\TaskManager\WpfTaskManager\bin\Debug\net8.0-windows\WpfTaskManager.exe";
string console = @"C:\S. KCK\Graficzny Poprawa\TaskManager\TaskManager\bin\Debug\net8.0\TaskManager.exe";

Console.WriteLine("1 - Console\n2 - WPF\n");

int r;
while (!int.TryParse(Console.ReadLine(), out r) || (r != 1 && r != 2))
    Console.WriteLine("Wrong number! Type 1 or 2.");

Process.Start(r == 1 ? console : wpf);
