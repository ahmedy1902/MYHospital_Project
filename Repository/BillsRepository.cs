using CareNet_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CareNet_System.Repository
{
    public class BillsRepository : IBillsRepository
    {
        private readonly HosPitalContext _context;

        public BillsRepository(HosPitalContext context)
        {
            _context = context;
        }

        public List<Bills> GetAll()
        {
            return _context.Bills
                .Include(b => b.patient)
                    .ThenInclude(p => p.followUpDoctor)
                .Select(b => new Bills
                {
                    Id = b.Id,
                    total_amount = b.total_amount,
                    Payment_Method = b.Payment_Method,
                    insurance_id = b.insurance_id ?? 0,
                    patient_id = b.patient_id ?? 0,
                    patient = b.patient ?? new Patient()
                }).ToList();
        }

        public Bills GetById(int id)
        {
            return _context.Bills
                .Include(b => b.patient)
                    .ThenInclude(p => p.followUpDoctor)
                .FirstOrDefault(b => b.Id == id);
        }

        public List<Bills> GetByPatientId(int patientId)
        {
            return _context.Bills.Where(b => b.patient_id == patientId).ToList();
        }

        public void Add(Bills entity)
        {
            _context.Bills.Add(entity);
            Save();
        }

        public void Update(Bills entity)
        {
            _context.Bills.Update(entity);
            Save();
        }

        public void Delete(int id)
        {
            var bill = _context.Bills.Find(id);
            if (bill != null)
            {
                _context.Bills.Remove(bill);
                Save();
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}