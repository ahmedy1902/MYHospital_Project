
namespace CareNet_System.Repository
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        void Add(T obj);
        void Save();
        void Delete(int id);
        void Update(T obj);
        T GetById(int id);
    }
}
