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
        private Users currentUser;

        public Users CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                // You can also add additional logic here if needed
            }
        }

        public bool IsUserAuthenticated => CurrentUser != null && CurrentUser.UserID != -1;

        public void ClearUser()
        {
            CurrentUser = null;
        }
    }
}
