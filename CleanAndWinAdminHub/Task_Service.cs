using Model;

namespace CleanAndWinAdminHub
{
    public class Task_Service
    {
        public List<Project_Task> Tasks {get;set;}

        public Task_Service()
        {
            Tasks = new List<Project_Task>();
        }
    }
}
