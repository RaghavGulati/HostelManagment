using HotelManagment.Database_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HotelManagment.Helpers
{
    public class Common
    {
        HostelManagmentEntities entity = new HostelManagmentEntities();
        public string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }

        public string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }

        public void ManageLogs(int userId, String desc)
        {
            AccessLog log = new AccessLog();
            log.IpAddress = GetIPAddress();
            log.UserId = userId;
            log.Description = desc;
            log.Time = DateTime.Now;
            entity.AccessLogs.Add(log);
            entity.SaveChanges();
        }

        public string GetIPAddress()
        {
            string IPAddress = string.Empty;
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }

        //public User GetCurrentUser()
        //{
        //    var user = (User)Session["CurrentUser"];
        //    return user;
        //}
    }
}