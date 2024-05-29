using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAndWinApp
{
    public class UserService
    {
        public Users CurrentUser { get; set; }

        

        public bool IsUserAuthenticated => CurrentUser != null && CurrentUser.UserID != -1;

        public void ClearUser()
        {
            CurrentUser = null;
        }
    }
}
