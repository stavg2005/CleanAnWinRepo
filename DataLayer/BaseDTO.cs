using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DataLayer
{
    public abstract class BaseDTO
    {

        protected readonly string _connectionString = "server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";


        public abstract  Task Insert(object o);
        public abstract Task Update(object o);
        public abstract Task Delete(int id);
        public abstract Task GetByPK(int Id);
        public abstract Task SelectAll();
    }
}
