using Model;

namespace CleanAndWinAdminHub
{
    public class Admin_Service
    {
        public Users admin { get; set; }

        public Admin_Service(Users admin)
        {
            this.admin = admin;
        }
        public Admin_Service()
        {
            admin = new Users();
        }
    }
}
