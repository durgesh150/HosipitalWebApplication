using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace HosipitalWebApplication.Migration
{
    public partial class HosplitalDbContext : DbContext
    {
        public HosplitalDbContext()
            : base("name=HosplitalDbContext")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Appuser> Appusers { get; set; }
        public virtual DbSet<Patientdata> Patientdatas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
