using Model;

namespace CleanAndWinAdminHub
{
    public class Admin_Service
    {
        public Admin admin { get; set; }

        public Admin_Service(Admin admin)
        {
            this.admin = admin;
        }
        public Admin_Service()
        {
            admin = new Admin();
        }
    }
}
