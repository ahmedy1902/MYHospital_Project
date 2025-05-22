using CareNet_System.Models;
using CareNet_System.Repository;
using Microsoft.EntityFrameworkCore;

public class StaffRepository : IRepository<Staff>
{
    private readonly HosPitalContext _context;

    public StaffRepository(HosPitalContext context)
    {
        _context = context;
    }

    public List<Staff> GetAll()
    {
        return _context.Staff.ToList();
    }

    public Staff GetById(int id)
    {
        return _context.Staff.FirstOrDefault(s => s.Id == id);
    }

    public void Add(Staff entity)
    {
        _context.Staff.Add(entity);
        Save();
    }

    public void Update(Staff entity)
    {
        _context.Staff.Update(entity);
        Save();
    }

    public void Delete(int id)
    {
        var staff = _context.Staff.Find(id);
        if (staff != null)
        {
            _context.Staff.Remove(staff);
            Save();
        }
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}