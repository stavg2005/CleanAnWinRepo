﻿using Model;
using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text.Json;
using System.Text;
using Google.Protobuf.WellKnownTypes;
using System.Globalization;
using static System.Net.WebRequestMethods;
using System.Net.Http;

namespace Model
{
    public class ApiServices 
    {
        private readonly HttpClient _httpClient;
        private string IPV4;
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

                string s = $"http://10.0.0.27:5087/api/CartControllercs/GetAllProducts?id={id}";
                var value = await _httpClient.GetFromJsonAsync<List<Product>>(s);
                value = value;
                return value;
            

        }

        public async Task<Users> Login(string email, string password)
        {
            string s = $"http://10.0.0.27:5087/api/Login/Login?email={email}&password={password}";
            return await _httpClient.GetFromJsonAsync<Users>(s);
        }

        public async Task<string> GetProfilePhoto(int id)
        {
            string s = $"http://10.0.0.27:5087/api/Upload/GetImageUri?id={id}";
            return await _httpClient.GetFromJsonAsync<string>(s);
        }
        
        public async Task<Tuple<string,string>> GetLatLngFromAPI(int id)
        {
            string s = "http://10.0.0.27:5087/api/Map/GetLocation?id=1";
            return await _httpClient.GetFromJsonAsync<Tuple<string, string>>(s);
        }

        public async Task<int> Register(string email, string password,string username,int location,int id)
        {
            string s = $"http://10.0.0.27:5087/api/Login/Register?username={username}&password={password}&email={email}&location={location}&id={id}";
            return await _httpClient.GetFromJsonAsync<int>(s);
        }

        public async Task<List<Locations>> GetAllLocations()
        {
            string s = "http://10.0.0.27:5087/GetAllLocation";
            return await _httpClient.GetFromJsonAsync<List<Locations>>(s);
        }

        public async Task<List<Product>> GetALLProducts()
        {
            string s = "http://10.0.0.27:5087/GetAllProducts";
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
                string apiurl = $"http://10.0.0.27:5087/ReportClean?userid={userid}&weight={Weight}&trashcanid={trashcanid}";

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
                string s = $"http://10.0.0.27:5087/GetProuctFromIndex?index={id}";
                return await (_httpClient.GetFromJsonAsync<Product>(s));
            }
            catch (Exception e)
            {
                return new Product(-1,"null","null",-1,null);
            }
        }
    }
  }