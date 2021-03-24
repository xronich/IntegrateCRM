using IntegrateCRM.Abstractions.DB;
using System.Data.Common;
using System.Data.Entity;
using System.Threading.Tasks;

namespace IntegrateCRM.Database
{
    public class CRMDbContext : DbContext, ICRMDBContext
    {
        public CRMDbContext Current => this;
        public CRMDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Configure();
        }

        public CRMDbContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {
            Configure();
        }

        private void Configure()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelConfiguration.Configure(modelBuilder);
            var initializer = new DbInitializer(modelBuilder);
            System.Data.Entity.Database.SetInitializer(initializer);
        }

        public async Task Insert<TEntity>(TEntity entity) where TEntity : class
        {
            base.Set<TEntity>().Add(entity);

            await SaveChangesAsync();
        }
    }
}