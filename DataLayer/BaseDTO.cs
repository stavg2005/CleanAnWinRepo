using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DataLayer
{
    public class BaseDTO
    {

        protected readonly string _connectionString = "server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";


        protected MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
