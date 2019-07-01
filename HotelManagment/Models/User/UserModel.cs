using HotelManagment.Database_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagment.Models.User
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }
         public DateTime Dob { get; set; }
        public string Gender { get; set; }        
        public List<Country> Countries { get; set; }
        public List<State> State { get; set; }
        public List<City> Cities { get; set; }
        public int AddressId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string PostCode { get; set; }
        public int CountryId { get; set; }
        public bool IsPrimary { get; set; }

    }
}