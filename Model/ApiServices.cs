using Model;
using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text.Json;
using System.Text;
using Google.Protobuf.WellKnownTypes;
using System.Globalization;
using static System.Net.WebRequestMethods;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace Model
{
    public class ApiServices 
    {
        private readonly HttpClient _httpClient;
        private string IPV4;
        private string Apiurl = "http://192.168.1.64:5087";
        public ApiServices()
        {
            _httpClient = new HttpClient();
            IPV4 = GetIpv4Address();

        }

        private string GetIpv4Address()
        {
            string ipv4Address = string.Empty;

            // Get the host name of the local machine
            string hostName = Dns.GetHostName();

            // Get the IP addresses associated with the host
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            // Find the first IPv4 address
            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipv4Address = address.ToString();
                    break;
                }
            }

            return ipv4Address;
        }
        public async Task<List<Product>> Getc(int id, bool isphone)
        {

                string s = $"{Apiurl}/api/CartControllercs/GetAllProducts?id={id}";
                var value = await _httpClient.GetFromJsonAsync<List<Product>>(s);
                value = value;
                return value;
            

        }

        public async Task<Users> Login(string email, string password)
        {
            string s = $"{Apiurl}/api/Login/Login?email={email}&password={password}";
            return await _httpClient.GetFromJsonAsync<Users>(s);
        }

        public async Task<string> GetProfilePhoto(int id)
        {
            string s = $"{Apiurl}/api/Upload/GetImageUri?id={id}";
            return await _httpClient.GetFromJsonAsync<string>(s);
        }
        
        public async Task<Tuple<string,string>> GetLatLngFromAPI(int id)
        {
            string s = $"{Apiurl}/api/Map/GetLocation?id={id}";
            return await _httpClient.GetFromJsonAsync<Tuple<string, string>>(s);
        }
        public async Task<Locations> GetLocationFromPK(int id)
        {
            string s = $"http://{Apiurl}/GetLocationFromPK?id={id}";
            return await _httpClient.GetFromJsonAsync<Locations>(s);
        }
        public async Task<string> Register(string email, string password, string username, int location, int id, byte[] imagedata)
        {

            Users users = new Users(id, email, username,await GetLocationFromPK(location));
            string s =
            JsonSerializer.Serialize(users);
            HttpContent content = new StringContent(s, System.Text.Encoding.UTF8);
            string apiurl = $"{Apiurl}/api/Login/Register?username={username}&password={password}&email={email}&location={location}&id={id}";
            HttpResponseMessage response = await _httpClient.PostAsync(apiurl, content);
            string r = await UploadImageToApi(id, imagedata);
            if (response.IsSuccessStatusCode && r == "Image uploaded successfully")
            {

                return ("POST request successful!");

            }
            else
            {
                return ($"POST request failed with status code: {response.StatusCode} and contect {await response.Content.ReadAsStringAsync()}");
            }

        }

        public async Task<List<Locations>> GetAllLocations()
        {
            string s = $"{Apiurl}/GetAllLocation";
            return await _httpClient.GetFromJsonAsync<List<Locations>>(s);
        }

        public async Task<List<Product>> GetALLProducts()
        {
            string s = $"{Apiurl}/GetAllProducts";
            return await _httpClient.GetFromJsonAsync<List<Product>>(s);
        }

        public async Task<string> ReportClean(int Weight, int userid, int trashcanid)
        {
            try
            {
                ReportClean r = new ReportClean(userid, trashcanid, Weight);
                string s = 
                    JsonSerializer.Serialize(r);
                Console.WriteLine(s);
                HttpContent content = new StringContent(s, System.Text.Encoding.UTF8);
                string apiurl = $"http://{Apiurl}:5087/ReportClean?userid={userid}&weight={Weight}&trashcanid={trashcanid}";

                HttpResponseMessage response = await _httpClient.PostAsync(apiurl, content);
                
                if (response.IsSuccessStatusCode)
                {
                    return ("POST request successful!");

                }
                else
                {
                    return ($"POST request failed with status code: {response.StatusCode} and contect {await response.Content.ReadAsStringAsync()}");
                }
            }
            // Check if the request was successful (status code 200-299)

            
            catch (Exception ex)
            {
                return($"An error occurred: {ex.Message}");


            }


        }

        public async Task<Product> GetProductFromPK(int id)
       {
            try
            {
                string s = $"{Apiurl}/GetProuctFromIndex?index={id}";
                return await (_httpClient.GetFromJsonAsync<Product>(s));
            }
            catch (Exception e)
            {
                return new Product(-1,"null","null",-1,null);
            }
        }

        public async Task<string> UpdatePassword(int id, string password)
        {
            try
            {
                
                string s =
                    JsonSerializer.Serialize(password);
                
                HttpContent content = new StringContent(s, System.Text.Encoding.UTF8);
                string apiurl = $"{Apiurl}/api/Login/UpdatePassword?id={id}&password={password}";
                HttpResponseMessage response = await _httpClient.PostAsync(apiurl, content);

                if (response.IsSuccessStatusCode)
                {
                    return ("POST request successful!");

                }
                else
                {
                    return ($"POST request failed with status code: {response.StatusCode} and contect {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                return ($"An error occurred: {ex.Message}");


            }
        }
        public async Task<string> UpdateUserName(int id, string username)
        {
            try
            {
                string s = JsonSerializer.Serialize(username);

                HttpContent content = new StringContent(s, System.Text.Encoding.UTF8);
                string apiurl = $"{Apiurl}/api/Login/UpdateUserName?id={id}&username={username}";

                HttpResponseMessage response = await _httpClient.PostAsync(apiurl, content);
               
                if (response.IsSuccessStatusCode)
                {
                    return ("POST request successful!");
                }
                else
                {
                    return ($"POST request failed with status code: {response.StatusCode} and content {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                return ($"An error occurred: {ex.Message}");
            }
        }

        public async Task<string> UpdateUserEmail(int id,string useremail)
        {
            try
            {
                string s = JsonSerializer.Serialize(useremail);

                HttpContent content = new StringContent(s, System.Text.Encoding.UTF8);
                string apiurl = $"{Apiurl}/api/Login/UpdateEmail?id={id}&useremail={useremail}";

                HttpResponseMessage response = await _httpClient.PostAsync(apiurl, content);

                if (response.IsSuccessStatusCode)
                {
                    return ("POST request successful!");
                }
                else
                {
                    return ($"POST request failed with status code: {response.StatusCode} and content {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                return ($"An error occurred: {ex.Message}");
            }
        }

        public async Task<string> UploadImageToApi(int userId, byte[] imageData)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                using (MultipartFormDataContent content = new MultipartFormDataContent())
                {
                    // Your API endpoint URL
                    string apiUrl = $"{Apiurl}/api/upload/uploadImage/{userId}";

                    // Convert byte array to Base64 string
                    string base64Image = Convert.ToBase64String(imageData);

                    // Add the image data as a binary content
                    ByteArrayContent imageContent = new ByteArrayContent(imageData);
                    content.Add(imageContent, "imageData", "image.jpg"); // "imageData" is the parameter name expected by the server

                    // Make the POST request
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Image uploaded successfully
                        
                        return ("Image uploaded successfully");

                    }
                    else
                    {
                        // Handle the error
                        return ($"Error: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return ($"Exception: {ex.Message}");
            }
        }

        public async Task<string> AddItemToCart(string p, int id)
        {
            try
            {
                string s = JsonSerializer.Serialize(p);

                HttpContent content = new StringContent(s, System.Text.Encoding.UTF8);
                string apiurl = $"{Apiurl}/api/CartControllercs/AddProductTocart?userid={id}&productid={p}";

                HttpResponseMessage response = await _httpClient.PostAsync(apiurl, content);

                if (response.IsSuccessStatusCode)
                {
                    return ("POST request successful!");
                }
                else
                {
                    return ($"POST request failed with status code: {response.StatusCode} and content {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                return ($"An error occurred: {ex.Message}");
            }
        }

       public async Task<List<Tuple<string,string>>> GetAllTrashCanLocations()
        {

                string apiurl = $"{Apiurl}/GetAllTrashCanLocations";

                List<Tuple<string, string>> l = (await _httpClient.GetFromJsonAsync<List<Tuple<string, string>>>(apiurl));
                return l;
                
            
        }

        

    }
}
