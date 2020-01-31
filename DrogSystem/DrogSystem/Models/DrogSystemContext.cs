using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace DrogSystem.Models
{
    public class DrogSystemContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public DrogSystemContext() : base("name=DrogSystemContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<DrogSystem.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<DrogSystem.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<DrogSystem.Models.Provider> Providers { get; set; }

        public System.Data.Entity.DbSet<DrogSystem.Models.PaymentProvider> PaymentProviders { get; set; }

        public System.Data.Entity.DbSet<DrogSystem.Models.Marker> Markers { get; set; }

        public System.Data.Entity.DbSet<DrogSystem.Models.ProductDetail> ProductDetails { get; set; }

        public System.Data.Entity.DbSet<DrogSystem.Models.Presentation> Presentations { get; set; }

        public System.Data.Entity.DbSet<DrogSystem.Models.ProductPresentationPrice> ProductPresentationPrices { get; set; }

    }
}
