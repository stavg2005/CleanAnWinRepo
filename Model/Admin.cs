using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Admin
    {
        public Admin(int id, string name, string bio, string position, byte[] profile, string phone, string email, List<Project_Task> tasks)
        {
            Id = id;
            Name = name;
            Bio = bio;
            Position = position;
            Profile = profile;
            this.phone = phone;
            Email = email;
            Tasks = tasks;
        }

        public Admin()
        {

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }

        public string Position {  get; set; }

        public byte[] Profile { get; set; }

        public string phone { get; set; }
        public string Email { get; set; }

        public List<Project_Task> Tasks { get; set; }
    }
}
