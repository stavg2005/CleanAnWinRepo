using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Project_Task
    {
        public Project_Task(int iD, string name, int completion, int budget)
        {
            ID = iD;
            Name = name;
            Completion = completion;
            Budget = budget;
        }

        public Project_Task()
        {
            ID = 0;
            Name = "TEMP";
            Completion = 0;
            Budget = 0;
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public List<Admin> Admins { get; set; }
        public int Completion {  get; set; }

        public int Budget {  get; set; }


        
    }
}
