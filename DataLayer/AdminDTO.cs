using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace DataLayer
{
    public class AdminDTO
    {

        public static async Task<Admin> Login(string Email, string password)
        {           
            string ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {

                string update = $"SELECT * FROM admin WHERE Password = '{password}' AND Email = '{Email}'";
                Admin ca = new Admin();
                try
                {
                    await connection.OpenAsync();

                    using (MySqlCommand cmd = new MySqlCommand(update, connection))
                    {
                        MySqlDataReader r = cmd.ExecuteReader();
                        
                        if (r.HasRows)
                        {
                            await r.ReadAsync();
                            int id = r.GetInt32(0);
                            ca = new Admin(id, r.GetString(1), r.GetString(7), r.GetString(4), new byte[0], r.GetString(6), r.GetString(2), new List<Project_Task>());

                        }
                        return ca;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return ca;
                }
            }
        }

        public static async Task<Admin> GetAdminById(int id)
        {
            string ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {

                string update = $"SELECT * FROM admin WHERE AdminID = '{id}'";
                Admin ca = new Admin();
                try
                {
                    await connection.OpenAsync();

                    using (MySqlCommand cmd = new MySqlCommand(update, connection))
                    {
                        MySqlDataReader r = cmd.ExecuteReader();

                        if (r.HasRows)
                        {
                            await r.ReadAsync();
                            ca = new Admin(id, r.GetString(1), r.GetString(7), r.GetString(4), new byte[0], r.GetString(6), r.GetString(2), new List<Project_Task>());

                        }
                        return ca;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return ca;
                }
            }
        }
    }
}
