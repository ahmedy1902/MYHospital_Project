using CareNet_System.Models;
using CareNet_System.Repository;

namespace CareNet_System.Repostatory
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Department GetById(int id);
    }
}
