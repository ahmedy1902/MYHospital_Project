using CareNet_System.Models;
using CareNet_System.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CareNet_System.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HosPitalContext _context;

        public DepartmentRepository(HosPitalContext context)
        {
            _context = context;
        }

        public List<Department> GetAll()
        {
            return _context.Departments.Include(d => d.staff).Include(d => d.patients).ToList();
        }

        public Department GetById(int id)
        {
            return _context.Departments.Include(d => d.staff).Include(d => d.patients)
                .FirstOrDefault(d => d.Id == id);
        }

        public void Add(Department entity)
        {
            _context.Departments.Add(entity);
            Save();
        }

        public void Update(Department entity)
        {
            _context.Departments.Update(entity);
            Save();
        }

        public void Delete(int id)
        {
            var dept = _context.Departments.Find(id);
            if (dept != null)
            {
                _context.Departments.Remove(dept);
                Save();
            }
        }
        public List<Department> GetByManager(string managerName)
        {
            return _context.Departments.Where(d => d.manager == managerName).ToList();
        }

        public List<Department> GetBySize(int minEmployees)
        {
            return _context.Departments.Where(d => d.employees_num >= minEmployees).ToList();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}