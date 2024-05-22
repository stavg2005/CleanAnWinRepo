using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using Services;
namespace CleanAndWinApp
{
    public class AuthService
    {
        public const string AuthTokenKey = "AuthToken";

        public bool IsUserAuthenticated => !string.IsNullOrEmpty(GetAuthToken());

        public string GetAuthToken()
        {

            var token = SecureStorage.GetAsync(AuthTokenKey).Result;
            return token;


        }


        public void SetAuthToken(string authToken)
        {
            SecureStorage.SetAsync(AuthTokenKey, authToken).Wait();
            Console.WriteLine($"AuthToken set: {authToken}");
        }

        public void ClearAuthToken()
        {
            SecureStorage.Remove(AuthTokenKey);
            Console.WriteLine("AuthToken cleared");
        }

        public async Task<Users> GetUserInfo()
        {
            ApiServices api = new ApiServices();
            
           
            String token = GetAuthToken();
            token = token;
            int id = await api.GetUserIDfromEmail(token);
            Users u = await api.GetUser(id);
            return u;
        }

        
    }
}
