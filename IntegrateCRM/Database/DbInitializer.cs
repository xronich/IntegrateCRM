using System.Data.Entity;
using SQLite.CodeFirst;
using SQLite.CodeFirst.NetCore.Console.Entity;

namespace IntegrateCRM.Database
{
    public class DbInitializer : SqliteDropCreateDatabaseWhenModelChanges<CRMDbContext>
    {
        public DbInitializer(DbModelBuilder modelBuilder) : base(modelBuilder, typeof(CustomHistory))
        { }

        protected override void Seed(CRMDbContext context)
        {
            // Here you can seed your core data if you have any.
        }
    }
}