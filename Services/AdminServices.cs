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
    public class AdminServices
    {
        private  readonly HttpClient _httpClient;
        private string Apiurl = "http://localhost:5086";

        public AdminServices(
            ) { _httpClient = new HttpClient(); }
        public  async Task<Admin> Login(string email, string password)
        {
            string s = $"{Apiurl}/api/AdminAPI/Login?Email={email}&Password={password}";
            try
            {
                return await _httpClient.GetFromJsonAsync<Admin>(s);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
