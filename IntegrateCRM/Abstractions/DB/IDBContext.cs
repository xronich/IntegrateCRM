using IntegrateCRM.Database;
using IntegrateCRM.Database.Entity;
using System.Threading.Tasks;

namespace IntegrateCRM.Abstractions.DB
{
    public interface ICRMDBContext
    {
        CRMDbContext Current { get; }
        Task Insert<TEntity>(TEntity entity) where TEntity : class;
    }
}
