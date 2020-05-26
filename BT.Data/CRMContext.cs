using BT.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Data
{
    public  class CRMContext :DbContext
    {
        public CRMContext(DbContextOptions<CRMContext> options):base(options)
        {
            
        }

        public DbSet<Employees> Employees { get; set; }

        public DbSet<Roles> Roles { get; set; }
        public DbSet<Employees_Roles> Employees_Roles { get; set; }

        public DbSet<Logs> Logs { get; set; }

        public DbSet<LogCategories> LogCategories { get; set; }

        public DbSet<Permissions> Permissions { get; set; }


        public DbSet<Roles_Permissions> Roles_Permissions { get; set; }

        public DbSet<PublicCounts> PublicCounts { get; set; }

        public DbSet<TrackRecords> TrackRecords { get; set; }

        public DbSet<Customers> Customers { get; set; }

        public DbSet<CustomerSegmentations> CustomerSegmentations { get; set; }

        public DbSet<Transforms_Customers> Transforms_Customers { get; set; }

        public DbSet<Products> Products { get; set; }

        public DbSet<Orders> Orders { get; set; }

        public DbSet<Orders_Products> Orders_Products { get; set; }
    }
}
