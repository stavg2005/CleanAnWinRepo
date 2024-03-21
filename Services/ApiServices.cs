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
using System.Drawing;
using DataLayer;
using System.Drawing.Printing;
using Services;
using Org.BouncyCastle.Asn1.Crmf;

namespace Services
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
            string s = $"{Apiurl}/GetLocationFromPK?id={id}";
            return await _httpClient.GetFromJsonAsync<Locations>(s);
        }
        public async Task<string> Register(string email, string password, string username, int location, byte[] imagedata)
        {
            // Create a new UsersDTO object
            UsersDTO user = new UsersDTO();

            // Set properties of the user object
            user.Email = email;
            user.Password = password;
            user.Username = username;
            user.UserLocation = location;
            user.ProfilePicture = imagedata;

            // Serialize the user object to JSON
            string serializedUser = JsonSerializer.Serialize(user);

            // Create HTTP content with the serialized user object
            HttpContent content = new StringContent(serializedUser, Encoding.UTF8, "application/json");

            // Prepare the API URL
            string apiurl = $"{Apiurl}/api/Login/Register";

            try
            {
                // Send the HTTP POST request to the API
                HttpResponseMessage response = await _httpClient.PostAsync(apiurl, content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    return "POST request successful!";
                }
                else
                {
                    // If the request failed, return the error message
                    return $"POST request failed with status code: {response.StatusCode} and content {await response.Content.ReadAsStringAsync()}";
                }
            }
            catch (Exception e)
            {
                // If an exception occurs, return the error message
                return e.Message;
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
                string apiurl = $"{Apiurl}/api/TrashCan/ReportClean";

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<ReportClean>(apiurl, r);
                
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

        public async Task<string> UpdateUser(Users u)
        {
            try
            {
                string api = $"{Apiurl}/api/Login/UpdateUser";
                UsersDTO U = new UsersDTO(u);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<UsersDTO>(api, U);
                if (response.IsSuccessStatusCode)
                {
                    return ("PUT request successful!");

                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return ($"PUT request failed with status code: {response.StatusCode} and contect {content}");
                }
            }
            catch (Exception ex)
            {
                return ($"An error occurred: {ex.Message}");


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

        public async Task<string> UploadProductImage(int ProductID, byte[] imageData)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                using (MultipartFormDataContent content = new MultipartFormDataContent())
                {
                    // Your API endpoint URL
                    string apiUrl = $"{Apiurl}/api/upload/UploadeProductImage/{ProductID}";

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

       public async Task<List<TrashCan>> GetAllTrashCanLocations()
        {
            List<TrashCan> l = new List<TrashCan>();
            try
            {
                string apiurl = $"{Apiurl}/api/TrashCan/GetAllTrashCanLocations";
                l = await _httpClient.GetFromJsonAsync<List<TrashCan>>(apiurl);
                return l;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
                return l;
            }

                
            
        }

        public async Task<string> UpdateUserCoin(int id,int coins)
        {
            try
            {
                string s = JsonSerializer.Serialize(coins);

                string url = $"{Apiurl}/api/Login/UpdateUserCoin?id={id}&coin={coins}";
                HttpContent content = new StringContent(s, System.Text.Encoding.UTF8);
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
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

        public async Task<string> DeleteUserCart(int id)
        {
            try
            {
                string s = JsonSerializer.Serialize(id);
                string url = $"{Apiurl}/api/Login/DeleteUserCart?id={id}";
                HttpContent content = new StringContent(s, System.Text.Encoding.UTF8);
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
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

        public async Task<string> UpdateProduct(Product P)
        {
            try
            {
                string url = $"{Apiurl}/UpdateProduct";
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync<Product>(url,P);
                if (response.IsSuccessStatusCode)
                {
                    return ("POST request successful!");
                }
                else
                {
                    return ($"POST request failed with status code: {response.StatusCode} and content {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch(Exception ex) 
            {
                return ($"An error occurred: {ex.Message}");
            }
        }

        public async Task<string> InsertTrashCan(TrashCan t)
        {
            try
            {
                string url = $"{Apiurl}/api/TrashCan/InsertTrashCan";
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<TrashCan>(url, t);
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
       
        public async Task<string> DeleteTrashCan(int id)
        {
            try
            {
                string url = $"{Apiurl}/api/TrashCan/DeleteTrashCan";
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<int>(url, id);
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
        public async Task<string> InsertNewProduct(Product p)
        {
            try
            {
                string url = $"{Apiurl}/InsertNewProduct";
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Product>(url, p);
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

        public async Task<string> DeleteProductFromUserCart(Tuple<int,int> userIDproductID)
        {

            try
            {

                string Serilize = JsonSerializer.Serialize(userIDproductID);
                string url = $"{Apiurl}/api/CartControllercs/DeleteProductFromUserCart";
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<Tuple<int,int>>(url, userIDproductID);
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


        public async Task<string> AddNewOrder(int UserID , List<Product> P)
        {
            OrderRequestModel request = new OrderRequestModel(UserID, P);
            try
            {
                string url = $"{Apiurl}/api/Order/AddOrderToUser";
                string Serilize = JsonSerializer.Serialize(request);
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync<OrderRequestModel>(url, request);
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


        public async Task<List<Order>> GetAllOrders(int userid)
        {
            try
            {
                string url = $"{Apiurl}/api/Order/GetAllOrders?userid={userid}";
                List<Order> l = await _httpClient.GetFromJsonAsync<List<Order>>(url);
                return l;
            }
            catch (Exception ex)
            {
                return (new List<Order>(0));
            }
        }
    }


}
