using System.Data.Entity;
using IntegrateCRM.Database.Entity;

namespace IntegrateCRM.Database
{
    public class ModelConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>();
            modelBuilder.Entity<Registration>();
            modelBuilder.Entity<EmailMessage>();
        }
    }
}
