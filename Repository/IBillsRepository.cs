using CareNet_System.Models;

namespace CareNet_System.Repository
{
    public interface IBillsRepository : IRepository<Bills>
    {
        Bills GetById(int id);  
    }
}