using CareNet_System.Models;

namespace CareNet_System.Repository
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Patient GetById(int id); 
    }
}