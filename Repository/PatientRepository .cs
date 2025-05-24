using CareNet_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CareNet_System.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HosPitalContext _context;

        public PatientRepository(HosPitalContext context)
        {
            _context = context;
        }

        public List<Patient> GetAll()
        {
            return _context.Patients
                .Include(p => p.department)
                .Include(p => p.followUpDoctor)
                .ToList();
        }
        public Patient GetById(int id)
        {
            return _context.Patients.Include(p => p.department)
                                    .Include(p => p.followUpDoctor) 
                                    .Include(p => p.bills)
                                    .FirstOrDefault(p => p.Id == id);
        }
        public List<Patient> GetByDepartmentId(int deptId)
        {
            return _context.Patients.Where(p => p.dept_id == deptId).ToList();
        }

        public void Add(Patient entity)
        {
            _context.Patients.Add(entity);
            Save();
        }

        public void Update(Patient entity)
        {
            _context.Patients.Update(entity);
            Save();
        }

        public void Delete(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                Save();
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}