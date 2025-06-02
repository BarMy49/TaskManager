using System.Windows;
using System.Windows.Controls;
using TaskManager.Controller;
using TaskManager.View;
using System.Windows.Input;
using Microsoft.Win32;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TaskManager.Localization;
using WpfTaskManager.Themes;
using Forms = System.Windows.Forms;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using CheckBox =  System.Windows.Controls.CheckBox;

namespace WpfTaskManager
{
    public partial class MainWindow : Window
    {
        private WpfView? view;
        private TaskController? controller;
        private readonly ILocalizer _localizer;
        private readonly IThemeManager _themeManager;
        private readonly HashSet<int> SelectedTasks = new HashSet<int>();
        private NotifyIcon _trayIcon;

        public MainWindow(ILocalizer localizer, IThemeManager themeManager)
        {
            InitializeComponent();
            _localizer = localizer;
            _themeManager = themeManager;
            WindowState = WindowState.Normal;
            ShowInTaskbar = false;
            Visibility = Visibility.Visible;
            // Create system tray icon
            _trayIcon = new NotifyIcon();
            _trayIcon.Icon = new System.Drawing.Icon("WpfTaskManager.ico"); // Add .ico file to project
            _trayIcon.Visible = true;
            _trayIcon.Text = "Task Manager";

            // Add context menu to tray icon
            var contextMenu = new System.Windows.Forms.ContextMenuStrip();
            contextMenu.Items.Add("Add Task", null, (s, e) => AddTask());
            contextMenu.Items.Add("Exit", null, (s, e) => ExitApp());

            _trayIcon.ContextMenuStrip = contextMenu;

            _trayIcon.DoubleClick += (s, e) => ToggleMainWindowVisibility();
        }
        private void ToggleMainWindowVisibility()
        {
            if (this.Visibility != Visibility.Visible || this.WindowState == WindowState.Minimized)
            {
                ShowMainWindow();
            }
            else
            {
                HideMainWindow();
            }
        }
        private void ShowMainWindow()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.ShowInTaskbar = false;
            this.Visibility = Visibility.Visible;
            this.Activate();
        }
        
        private void HideMainWindow()
        {
            this.WindowState = WindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Visibility = Visibility.Hidden;
        }

        private void ExitApp()
        {
            _trayIcon.Visible = false;
            System.Windows.Application.Current.Shutdown();
        }

        protected override void OnClosed(EventArgs e)
        {
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
            base.OnClosed(e);
        }

        public void SetView(WpfView view, TaskController controller)
        {
            this.controller = controller;
            DataContext = this.view = view;
            controller?.ListAllTasks();

            //Initial button visibility
            AllTasksButton.Visibility = Visibility.Collapsed;
            EditTaskButton.Visibility = Visibility.Collapsed;
            DeleteTaskButton.Visibility = Visibility.Collapsed;
            ToggleTaskButton.Visibility = Visibility.Collapsed;
        }

        public void RefreshColumnHeaders()
        {
            // ID nie tłumaczymy
            TasksGrid.Columns[2].Header = Application.Current.Resources["TitleText"];
            TasksGrid.Columns[3].Header = Application.Current.Resources["DescriptionText"];
            TasksGrid.Columns[4].Header = Application.Current.Resources["StatusText"];
            TasksGrid.Columns[5].Header = Application.Current.Resources["CategoryText"];
            TasksGrid.Columns[6].Header = Application.Current.Resources["PriorityText"];
            TasksGrid.Columns[7].Header = Application.Current.Resources["DueDateText"];
            
            TasksGrid.Items.Refresh();
            controller.ListAllTasks();
        }

        public void RefreshControllerBindings()
        {
            // Ponownie ustaw DataContext
            DataContext = view;

            // Uaktualnij bindowania kontrolek
            if (TasksGrid != null)
            {
                TasksGrid.ItemsSource = null;
                TasksGrid.ItemsSource = view?.Records;
            }
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            view?.SetKey(System.ConsoleKey.UpArrow);
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            view?.SetKey(System.ConsoleKey.DownArrow);
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            view?.SetKey(System.ConsoleKey.Enter);
        }

        private void ListAllTasks_Click(object sender, RoutedEventArgs e)
        {
            view.Ignore = true;
            controller?.ListAllTasks();
            AllTasksButton.Visibility =  Visibility.Collapsed;
            ListIncompleteButton.Visibility =  Visibility.Visible;
            ListIncompleteButton.Visibility =  Visibility.Visible;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            // view.DeleteVisibility = Visibility.Collapsed;
            // view.ToggleVisibility = Visibility.Collapsed;
            // view.EditVisibility = Visibility.Collapsed;
            // view.CategoryFilterVisibility = Visibility.Collapsed;
            // view.FieldsVisibility = Visibility.Visible;
            // view.AddConfirmVisibility = Visibility.Visible;
            // view.EditConfirmVisibility = Visibility.Collapsed;

            AddTask();
        }

        private void AddTask()
        {
            var window = new AuxiliaryWindow("A", _localizer, controller, _themeManager, view) { Owner = this };
            window.ShowDialog();

            view.ClearTaskFields();
            view.Ignore = true;
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            // view.DeleteVisibility = Visibility.Collapsed;
            // view.ToggleVisibility = Visibility.Collapsed;
            // view.CategoryFilterVisibility = Visibility.Collapsed;
            //
            // view.FieldsVisibility = Visibility.Visible;
            // view.AddConfirmVisibility = Visibility.Collapsed;
            // view.EditConfirmVisibility = Visibility.Visible;
            //
            // view.EditVisibility = Visibility.Visible;
            view.EditId = null;

            var taskToEdit = view.Records
                .Where(task => SelectedTasks.Contains(task.Id))
                .ToList();  // ważne, ToList() tworzy nową listę kopię

            if (view != null && controller != null && taskToEdit.Count == 1)
            {
                view.EditId = taskToEdit[0].Id.ToString();
                
                var window = new AuxiliaryWindow("E", _localizer, controller, _themeManager, view) { Owner = this };
                window.ShowDialog();
            }
            else
            {
                return;
            }

            view.ClearTaskFields();
            view.Ignore = true;
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            // view.FieldsVisibility = Visibility.Collapsed;
            // view.AddConfirmVisibility = Visibility.Collapsed;
            // view.EditConfirmVisibility = Visibility.Collapsed;
            //
            // view.ToggleVisibility = Visibility.Collapsed;
            // view.EditVisibility = Visibility.Collapsed;
            // view.CategoryFilterVisibility = Visibility.Collapsed;
            //
            // view.DeleteVisibility = Visibility.Visible;
            view.DeleteId = null;

            view.Ignore = true;
            
            DeleteItem();
        }

        private void ToggleTaskCompletion_Click(object sender, RoutedEventArgs e)
        {
            // view.FieldsVisibility = Visibility.Collapsed;
            // view.AddConfirmVisibility = Visibility.Collapsed;
            // view.EditConfirmVisibility = Visibility.Collapsed;
            // view.DeleteVisibility = Visibility.Collapsed;
            // view.EditVisibility = Visibility.Collapsed;
            // view.CategoryFilterVisibility = Visibility.Collapsed;
            // view.ToggleVisibility = Visibility.Visible;
            view.ToggleId = null;

            view.Ignore = true;
            
            ToggleCompletion();
        }

        private void FilterTasksBy_Click(object sender, RoutedEventArgs e)
        {
            // Sneaky inne formularze
            // view.DeleteVisibility = Visibility.Collapsed;
            // view.ToggleVisibility = Visibility.Collapsed;
            // view.EditVisibility = Visibility.Collapsed;
            // view.FieldsVisibility = Visibility.Collapsed;
            // view.AddConfirmVisibility = Visibility.Collapsed;
            // view.EditConfirmVisibility = Visibility.Collapsed;

            // Nowy border do filtra kategorii
            // view.CategoryFilterVisibility = Visibility.Visible;
            // view.CategoryFilterText = null;

            var window = new FilterWindow(_localizer, controller, _themeManager, view) { Owner = this };
            window.ShowDialog();
            
            view.Ignore = true;
            AllTasksButton.Visibility = Visibility.Visible;
        }

        private void ListIncompleteTasks_Click(object sender, RoutedEventArgs e)
        {
            view.Ignore = true;
            controller?.ListIncompleteTasks();
            AllTasksButton.Visibility = Visibility.Visible;
            ListIncompleteButton.Visibility = Visibility.Collapsed;
            ListCompleteButton.Visibility = Visibility.Visible;
        }
        
        private void ListCompleteTasks_Click(object sender, RoutedEventArgs e)
        {
            view.Ignore = true;
            controller?.ListCompleteTasks();
            AllTasksButton.Visibility = Visibility.Visible;
            ListIncompleteButton.Visibility = Visibility.Visible;
            ListCompleteButton.Visibility = Visibility.Collapsed;
        }

        // private void ListTasksByPriority_Click(object sender, RoutedEventArgs e)
        // {
        //     view.Ignore = true;
        //     controller?.ListTasksByPriority();
        // }

        // private void SearchTasks_Click(object sender, RoutedEventArgs e)
        // {
        //     view.Ignore = true;
        //     controller?.SearchTasks();
        // }

        // private void Exit_Click(object sender, RoutedEventArgs e)
        // {
        //     view.Ignore = true;
        //     controller?.Exit();
        // }

        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        // Obsługa Enter w polu "Szukaj" (u góry)
        private void SearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                if (view != null && controller != null)
                {
                    view.Ignore = true;
                    controller.SearchTasks();
                }
            }
        }

        // ============================ USUWANIE ============================
        // private void DeleteId_PreviewKeyDown(object sender, KeyEventArgs e)
        // {
        //     if (e.Key == Key.Enter)
        //     {
        //         e.Handled = true;
        //         DeleteItem();
        //     }
        // }

        // private void DeleteIdButton_Click(object sender, RoutedEventArgs e)
        // {
        //     DeleteItem();
        // }

        // private void DeleteItem()
        // {
        //     if (view != null && controller != null)
        //     {
        //         view.Ignore = true;
        //         controller.DeleteTask();
        //         view.DeleteVisibility = Visibility.Collapsed;
        //         view.DeleteId = null;
        //     }
        // }

        private void DeleteItem()
        {
            var tasksToDelete = view.Records
                .Where(task => SelectedTasks.Contains(task.Id))
                .ToList();  // ważne, ToList() tworzy nową listę kopię
            
            if (view != null && controller != null && tasksToDelete.Count > 0)
            {
                view.Ignore = true;

                if (view?.Records != null)
                {
                    foreach (var task in tasksToDelete)
                    {
                        view.DeleteId = task.Id.ToString();
                        controller.DeleteTask();
                    }
                }
                controller?.ListAllTasks();
                SelectedTasks.Clear();
                TasksGrid.Items.Refresh();
                view.DeleteId = null;
                view.DisplayMessage($"{_localizer.GetString("TasksDeleted")}: {string.Join(", ", tasksToDelete.Select(t => t.Id))}");
            }
        }

        // ============================ TOGGLE ============================
        // private void ToggleId_PreviewKeyDown(object sender, KeyEventArgs e)
        // {
        //     if (e.Key == Key.Enter)
        //     {
        //         e.Handled = true;
        //         ToggleCompletion();
        //     }
        // }
        //
        // private void ToggleIdButton_Click(object sender, RoutedEventArgs e)
        // {
        //     ToggleCompletion();
        // }

        private void ToggleCompletion()
        {
            var tasksToToggle = view.Records
                .Where(task => SelectedTasks.Contains(task.Id))
                .ToList();  // ważne, ToList() tworzy nową listę kopię
            
            if (view != null && controller != null && tasksToToggle.Count > 0)
            {
                view.Ignore = true;
                
                if (view?.Records != null)
                {
                    foreach (var task in tasksToToggle)
                    {
                        view.ToggleId = task.Id.ToString();
                        controller.ToggleTaskCompletion();
                    }
                }
                controller?.ListAllTasks();
                // view.ToggleVisibility = Visibility.Collapsed;
                SelectedTasks.Clear();
                TasksGrid.Items.Refresh();
                view.ToggleId = null;
                view.DisplayMessage($"{_localizer.GetString("TasksChanged")} {string.Join(", ", tasksToToggle.Select(t => t.Id))}");
            }
        }


        // ============================ EDYCJA ============================
        // private void EditId_PreviewKeyDown(object sender, KeyEventArgs e)
        // {
        //     if (e.Key == Key.Enter)
        //     {
        //         e.Handled = true;
        //         SearchById_Click(sender, e);
        //     }
        // }

        private void SearchById_Click(object sender, RoutedEventArgs e)
        {
            if (view != null && controller != null)
            {
                view.Ignore = true;
                controller.SearchTaskById();
            }
        }
        
        // private void AddConfirm_Click(object sender, RoutedEventArgs e)
        // {
        //     if (view != null && controller != null)
        //     {
        //         view.Ignore = true;
        //         controller.AddTask();
        //
        //         view.FieldsVisibility = Visibility.Collapsed;
        //         view.AddConfirmVisibility = Visibility.Collapsed;
        //     }
        // }

        // private void EditConfirm_Click(object sender, RoutedEventArgs e)
        // {
        //     if (view != null && controller != null)
        //     {
        //         view.Ignore = true;
        //         controller.EditTask();
        //
        //         view.FieldsVisibility = Visibility.Collapsed;
        //         view.EditConfirmVisibility = Visibility.Collapsed;
        //         view.EditVisibility = Visibility.Collapsed;
        //     }
        // }

        // ============================ FILTR KATEGORII ============================
        // Obsługa Enter w polu "CategoryFilterText"
        // private void CategoryFilterText_PreviewKeyDown(object sender, KeyEventArgs e)
        // {
        //     if (e.Key == Key.Enter)
        //     {
        //         e.Handled = true;
        //         FilterCategory();
        //     }
        // }

        // Obsługa kliknięcia przycisku "Szukaj"
        // private void CategoryFilterTextButton_Click(object sender, RoutedEventArgs e)
        // {
        //     FilterCategory();
        // }

        // Metoda wywołująca logikę filtra w kontrolerze
        // private void FilterCategory()
        // {
        //     if (view != null && controller != null)
        //     {
        //         view.Ignore = true;
        //         controller.FilterTasks(0);
        //
        //         view.CategoryFilterVisibility = Visibility.Collapsed;
        //         view.CategoryFilterText = null;
        //     }
        // }

        // =============================== ADDED ============================

        private void ExportPdf_Click(object sender, RoutedEventArgs e)
        {
            var tasks = view?.Records;
            if (tasks == null || tasks.Count == 0)
            {
                MessageBox.Show(_localizer.GetString("NoTasksToExport"));
                return;
            }

            var dlg = new SaveFileDialog
            {
                Filter = "PDF (*.pdf)|*.pdf",
                FileName = "Tasks.pdf"
            };
            if (dlg.ShowDialog() != true) return;

            var columnKeys = new[] { "Title", "Description", "Status", "Category", "Priority", "DueDate" };

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    page.Header()
                        .Text(_localizer.GetString("TaskList"))
                        .SemiBold().FontSize(18).FontColor(Colors.Blue.Darken2).AlignCenter();

                    page.Content().Table(table =>
                    {
                        // Kolumny: proporcje
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1.2f); // Title
                            columns.RelativeColumn(2f); // Description
                            columns.RelativeColumn(); // Status
                            columns.RelativeColumn(); // Category
                            columns.RelativeColumn(); // Priority
                            columns.RelativeColumn(); // DueDate
                        });

                        // Nagłówki
                        table.Header(header =>
                        {
                            foreach (var key in columnKeys)
                            {
                                header.Cell().Element(HeaderCellStyle)
                                    .Text(_localizer.GetString(key)).Bold().FontColor(Colors.White);
                            }
                        });

                        // Wiersze z danymi
                        bool alternate = false;
                        foreach (var task in tasks)
                        {
                            var bgColor = alternate ? Colors.Grey.Lighten3 : Colors.White;
                            alternate = !alternate;

                            table.Cell().Element(c => CellStyle(c, bgColor)).Text(task.Title).SemiBold();
                            table.Cell().Element(c => CellStyle(c, bgColor)).Text(task.Description)
                                .WrapAnywhere().AlignLeft();
                            table.Cell().Element(c => CellStyle(c, bgColor)).Text(
                                task.IsCompleted
                                    ? _localizer.GetString("Completed")
                                    : _localizer.GetString("NotCompleted"));
                            table.Cell().Element(c => CellStyle(c, bgColor)).Text(task.Category);
                            table.Cell().Element(c => CellStyle(c, bgColor)).Text(task.Priority.ToString());
                            table.Cell().Element(c => CellStyle(c, bgColor))
                                .Text(task.DueDate?.ToString("dd-MM-yyyy") ?? "");
                        }
                    });

                    page.Footer().Row(row =>
                    {
                        row.RelativeColumn().AlignLeft().Text(text =>
                        {
                            text.Span(_localizer.GetString("GeneratedOn") + " ").FontSize(8);
                            text.Span(DateTime.Now.ToString("dd-MM-yyyy HH:mm")).FontSize(8).SemiBold();
                        });

                        row.ConstantColumn(100).AlignRight().Text(x =>
                        {
                            x.CurrentPageNumber().FontSize(8);
                            x.Span(" / ").FontSize(8);
                            x.TotalPages().FontSize(8);
                        });
                    });
                });
            }).GeneratePdf(dlg.FileName);

            MessageBox.Show(_localizer.GetString("ExportSuccess"));

            IContainer HeaderCellStyle(IContainer container) =>
                container
                    .Background(Colors.Blue.Medium)
                    .PaddingVertical(5)
                    .PaddingHorizontal(4)
                    .AlignCenter()
                    .ShowOnce();

            IContainer CellStyle(IContainer container, string bgColor) =>
                container
                    .Background(bgColor)
                    .PaddingVertical(4)
                    .PaddingHorizontal(4)
                    .AlignLeft()
                    .AlignMiddle();
        }

        private void TaskCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.Tag is int id)
                SelectedTasks.Add(id);
            if (SelectedTasks.Count == 1)
                EditTaskButton.Visibility = Visibility.Visible;
            else
                EditTaskButton.Visibility = Visibility.Collapsed;
            if (SelectedTasks.Count > 0)
            {
                DeleteTaskButton.Visibility = Visibility.Visible;
                ToggleTaskButton.Visibility = Visibility.Visible;
            }
        }

        private void TaskCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.Tag is int id)
                SelectedTasks.Remove(id);
            if (SelectedTasks.Count == 1)
                EditTaskButton.Visibility = Visibility.Visible;
            else
                EditTaskButton.Visibility = Visibility.Collapsed;
            if (SelectedTasks.Count < 1)
            {
                DeleteTaskButton.Visibility = Visibility.Collapsed;
                ToggleTaskButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}