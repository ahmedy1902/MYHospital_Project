using CareNet_System.Models;
using System.Collections.Generic;

namespace CareNet_System.Repository
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Department GetById(int id);
        List<Department> GetByManager(string managerName);
        List<Department> GetBySize(int minEmployees);  
    }
}