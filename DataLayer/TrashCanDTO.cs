﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Model;
using MySqlConnector;

namespace DataLayer
{
    public class TrashCanDTO
    {
        public int ID;
        public int Location;
        public int IsFull;
		public int Weight;
		public string Coordinates;

		public TrashCanDTO(int iD, int location, int isFull, int weight, string coordinates)
		{
			ID = iD;
			Location = location;
			IsFull = isFull;
			Weight = weight;
			Coordinates = coordinates;
		}

		public TrashCanDTO()
		{
			ID = -1;
			Location = -1;
			IsFull = -1;
			Weight = -1;
			Coordinates = "";
		}

        public static async Task ReportClean(int userid,int weight,int trashcanID)
        {
            try
            {
                DateTime now = DateTime.Now;
                string formattedDate = now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                string q = $"Insert into clean_report (UserID,TrashCanID,Weight,Date) values ('{userid}','{trashcanID}','{weight}','{formattedDate}')";
                MySqlConnection c = new MySqlConnection();
                c.ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
                c.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = c;
                cmd.CommandText = q;
                await cmd.ExecuteNonQueryAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

		private static string GetLatByPK(int id)
		{
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            int userId = id; // Replace with the UserID you want to use.

            string lat = "";
            string Lng = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT Lat from trashcan where TrashCanID ={id};";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lat = reader.GetString(0);
                        }
                    }
                }
                connection.Close();
                return lat;
            }
            
        }

        private static string GetLngByPK(int id)
        {
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            int userId = id; // Replace with the UserID you want to use.

            string Lng = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT Lng from trashcan where TrashCanID ={id};";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {


                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Lng = reader.GetString(0);
                        }
                    }
                }
                connection.Close();
                return Lng;
            }
        }

        public static Tuple<string,string> GetLatLngFromPK(int id)
        {
            string Lat = GetLatByPK(id);
            string lng = GetLngByPK(id);
            Tuple<string, string> t = new Tuple<string, string>(Lat, lng);
            return t;
        }

        public static int countRows()
        {
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            int num = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT * from trashcan;";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {


                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            num = num + 1;
                           
                        }
                    }
                }
               
                return num;
            }
        }
        public static async Task<List<TrashCan>> GetAlltrashCanLocations()
        {
            int i = 1;
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            List<TrashCan> l = new List<TrashCan>(countRows());
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT * from trashcan;";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {


                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            int isfull = int.Parse(reader.GetString(1));
                            int weight = int.Parse(reader.GetString(2));
                            string lng = reader.GetString(4);
                            string lat = reader.GetString(3);
                            l.Add(new TrashCan(id,isfull,weight,lng,lat));

                        }
                    }
                }

               
            }

            return l;

        }

        public static async Task InsertTrashCan(TrashCan trashCan)
        {
            string connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog"; 

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $"Insert into trashcan  (TrashCanIsFull,TrashCanWeight,Lat,Lng) VALUES ('{trashCan.isfull.ToString()}', '{trashCan.weight.ToString()}','{trashCan.latitude}','{trashCan.longitude}');";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        cmd.ExecuteNonQuery();

                    }
                    connection.Close();

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public static async Task DeleteTrashCan(int id)
        {
            String connectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $"Delete from trashcan where TrashCanID ={id};";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        cmd.ExecuteNonQuery();

                    }
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
	}
}
