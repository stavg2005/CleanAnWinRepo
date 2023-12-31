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
    }
}
