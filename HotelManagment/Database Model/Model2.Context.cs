﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HostelManagmentEntities : DbContext
    {
        public HostelManagmentEntities()
            : base("name=HostelManagmentEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AccessLog> AccessLogs { get; set; }
        public virtual DbSet<Bed> Beds { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<HostelRoom> HostelRooms { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<User_Address> User_Address { get; set; }
        public virtual DbSet<User_Courses> User_Courses { get; set; }
        public virtual DbSet<User_Room_Bed> User_Room_Bed { get; set; }
        public virtual DbSet<User_Rooms> User_Rooms { get; set; }
    }
}