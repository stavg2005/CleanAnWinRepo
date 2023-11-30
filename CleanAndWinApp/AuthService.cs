using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
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
            Tuple<string,string> t = await GetEmailAndPasswordFromToken();
            Users u = await api.Login(t.Item1, t.Item2);
            return u;
        }

        private async Task<Tuple<string,string>> GetEmailAndPasswordFromToken()
        {
            int index = 0;
            string key = GetAuthToken();
            for (int i=0;i< key.Length; i++)
            {
                if (key[i] == ',')
                {
                    index = i;
                }
            }
            string Email = key.Substring(0, index);
            string password = key.Substring(index+1);
            return new Tuple<string,string>(Email, password);

        }
    }
}
