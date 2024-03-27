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

        int ID { get; set; }
        string Name { get; set; }

        List<Admin> Admins { get; set; }
        int Completion {  get; set; }

        int Budget {  get; set; }


        
    }
}
