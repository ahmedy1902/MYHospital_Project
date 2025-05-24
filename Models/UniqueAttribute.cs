using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CareNet_System.Models;

namespace CareNet_System.Models
{
    public class UniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            Department deptFromReq = validationContext.ObjectInstance as Department;

            var _context = validationContext.GetService(typeof(HosPitalContext)) as HosPitalContext;

            if (_context == null) return new ValidationResult("Database context is not available.");

            Department deptFromDB = _context.Departments.FirstOrDefault(d => d.name == deptFromReq.name);

            if (deptFromDB == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"The department {deptFromDB.name} already exists.");
            }
        }
    }
}