//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelManagment.Database_Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class AccessLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string IpAddress { get; set; }
        public Nullable<System.DateTime> LoginTime { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    
        public virtual User User { get; set; }
    }
}
