using System.Threading.Tasks;

namespace project_vega.Core
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }

    
}