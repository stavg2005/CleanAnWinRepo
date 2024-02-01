using Model;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using MySql.Data.MySqlClient;
using DataLayer;

namespace Test
{
    class Program
    {
        //static void Main()
        //{
        //    //string adapterName = "Ethernet adapter vEthernet (Default Switch)";
        //    //string ipv4Address = GetIPv4Address(adapterName);

        //    //if (!string.IsNullOrEmpty(ipv4Address))
        //    //{
        //    //    Console.WriteLine($"IPv4 Address for {adapterName}: {ipv4Address}");
        //    //}
        //    //else
        //    //{
        //    //    Console.WriteLine($"IPv4 Address for {adapterName} not found.");
        //    //}
        //    //List<Locations> l = LocationsDTO.GetAllLocations();
        //    //foreach (Locations ol in l)
        //    //{
        //    //    Console.WriteLine( ol.Name);
        //    //}
        //}

        static async Task Main(string[] args)
        {
            //Model.ApiServices a = new Model.ApiServices();
            ////Console.WriteLine(await a.ReportClean(1255, 1, 1));
            ////Console.ReadLine();
            //Product p = await a.GetProductFromPK(1);
            //if (p != null)
            //{
            //    Console.WriteLine(p.ProductName);
            //    Console.ReadLine();
            //}

            //Model.ApiServices a = new Model.ApiServices();
            //List<Tuple<string,string>> l = await a.GetAllTrashCanLocations();
            //foreach (Tuple<string,string> i in l){
            //    Console.WriteLine("lat" + i.Item1);
            //    Console.WriteLine("lng" + i.Item2);
            //}
            //Console.ReadLine();
            Model.ApiServices a = new Model.ApiServices();
            //byte[] A = new byte[37357];
            //Product p = new Product(1,"PST-TEST","FDAFG",39552,A);
            //Console.WriteLine(await a.UpdateProduct(p));
            //Console.ReadLine();

            TrashCan t = new TrashCan();
            Console.WriteLine(await a.InsertTrashCan(t));
            Console.ReadLine();

        }

        static string GetIPv4Address(string adapterName)
        {
            string ipv4Address = string.Empty;

            // Get all network interfaces on the machine
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface nic in networkInterfaces)
            {
                if (nic.Description.Equals(adapterName, StringComparison.OrdinalIgnoreCase) &&
                    nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    // Get the IP properties of the selected network interface
                    IPInterfaceProperties ipProperties = nic.GetIPProperties();

                    // Find the first IPv4 address in the list
                    foreach (UnicastIPAddressInformation ip in ipProperties.UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            ipv4Address = ip.Address.ToString();
                            break;
                        }
                    }

                    break; // Stop searching after finding the specified adapter
                }
            }

            return ipv4Address;
        }
    }

}

