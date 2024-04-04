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
    public class LocationsDTO
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



        public static async Task<List<Locations>> GetAllLocations()
        {
            List<Locations> locations = new List<Locations>();

            using (MySqlConnection connection = new MySqlConnection(@"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog"))
            {
                connection.Open();
                string query = "SELECT LocationID, LocationName,lat,lng FROM location";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader =  command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Locations location = new Locations
                            {
                                ID = Convert.ToInt32(reader["LocationID"]),
                                Name = Convert.ToString(reader["LocationName"]),
                                lat = Convert.ToString(reader["lat"]),
                                lng = Convert.ToString(reader["lng"])
                                
                            };

                            locations.Add(location);
                        }
                    }
                }
            }
            return locations;
        }

        public static async Task<Locations> GetLocationFromPK(int id)
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
                                ID = Convert.ToInt32(reader["LocationID"]),
                                Name = Convert.ToString(reader["LocationName"]),
                                lat = Convert.ToString(reader["lat"]),
                                lng = Convert.ToString(reader["lng"])

                            };
                            locations = locationtemp;
                            
                        }
                    }
                }
            }
            return locations;

        }

        public static async Task UpdateLocation(Locations l)
        {
           
                string ConnectionString = @"server=localhost;user id=root;persistsecurityinfo=True;database=project;password=josh17rog";
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
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
        
    }
}
