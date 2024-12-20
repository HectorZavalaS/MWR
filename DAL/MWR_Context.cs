using MWR.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MWR.DAL
{
    public class MWR_Context : DbContext
    {
        public MWR_Context() : base ("MWR")
        {
        }

        public DbSet<Reportes> Reportes { get; set; }
        public DbSet<StoreIn> StoreIns { get; set; }
        public DbSet<MaterialOut> MaterialOuts { get; set; }
        public DbSet<FGInfo> FGInfos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}