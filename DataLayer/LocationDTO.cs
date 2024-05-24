using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace DataLayer
{
    public class LocationsDTO:BaseDTO
    {
        public int ID;
        public string Name;
        public string lat;
        public string lng;

        public LocationsDTO(int iD, string name)
        {
            ID = iD;
            Name = name;
        }

        public LocationsDTO()
        {
            ID = -1;
            Name = null;
        }



        public override async Task<List<Locations>> SelectAll()
        {
            List<Locations> locations = new List<Locations>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(@"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog"))
                {
                    await connection.OpenAsync();
                    string query = "SELECT LocationID, LocationName, lat, lng FROM location";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                Locations loc = new Locations
                                {
                                    ID = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    lat = reader.GetString(2),
                                    lng = reader.GetString(3)
                                };

                                locations.Add(loc);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("An error occurred: " + ex.Message);
                
            }
            return locations; 
        }
        public override   async Task<Locations> GetByPK(int id)
        {
            Locations locations = new Locations();

            using (MySqlConnection connection = new MySqlConnection(@"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog"))
            {
                connection.Open();
                string query = $"SELECT LocationID, LocationName,lat,lng FROM location where LocationID = '{id}'";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            Locations locationtemp = new Locations
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                lat = reader.GetString(2),
                                lng = reader.GetString(3)

                            };
                            locations = locationtemp;
                            
                        }
                    }
                }
            }
            return locations;

        }

        public override async Task Update(object o)
        {
                Locations l = (Locations)o;
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {

                    string update = $"UPDATE Location SET LocationName = @LocationName, lat = @lat, lng = @lng WHERE LocationID = {l.ID}; ";

                    try
                    {
                        await connection.OpenAsync();

                        using (MySqlCommand UpdateLocationCommand = new MySqlCommand(update, connection))
                        {
                            UpdateLocationCommand.Parameters.AddWithValue("@LocationName",l.Name);
                            UpdateLocationCommand.Parameters.AddWithValue("@lat", l.lat);
                            UpdateLocationCommand.Parameters.AddWithValue("@lng", l.lng);


                            await UpdateLocationCommand.ExecuteNonQueryAsync();
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
         }

        public override async Task Insert(object o)
        {
            Locations l = (Locations)o;
            string ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {

                string update = $"Insert into Location (LocationName,lat,lng) VALUES  (@LocationName, @lat, @lng);  ";

                try
                {
                    await connection.OpenAsync();

                    using (MySqlCommand UpdateLocationCommand = new MySqlCommand(update, connection))
                    {
                        UpdateLocationCommand.Parameters.AddWithValue("@LocationName", l.Name);
                        UpdateLocationCommand.Parameters.AddWithValue("@lat", l.lat);
                        UpdateLocationCommand.Parameters.AddWithValue("@lng", l.lng);


                        await UpdateLocationCommand.ExecuteNonQueryAsync();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public override async Task Delete(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = $"Delete from location where LocationID ={id};";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        await cmd.ExecuteNonQueryAsync();

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
