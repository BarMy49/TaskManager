namespace TaskManager.View
{
    public interface IView
    {
        void Center(string message);
        void DisplayFilterMenu(string[] options, int selectedOption);
        void DisplayMenu(string[] options, int selectedOption);
        void DisplayMessage(string message);
        void DisplayTasks(List<Model.TaskModel> tasks);
        string GetFilterCategory();
        bool GetFilterCompletionStatus();
        DateTime GetFilterDate();
        Model.TaskModel GetNewTaskDetails();
        string GetSearchKeyword();
        int GetTaskId();
        Model.TaskModel GetUpdatedTaskDetails(Model.TaskModel task);
        int GetUserChoice();
    }
}
