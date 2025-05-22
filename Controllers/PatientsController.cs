using Microsoft.AspNetCore.Mvc;
using CareNet_System.Models;
using CareNet_System.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using CareNet_System.Repository;
using Microsoft.AspNetCore.Authorization;

namespace CareNet_System.Controllers
{
    [Authorize]

    public class PatientsController : Controller
    {
        private readonly IPatientRepository _patientRepo;
        private readonly IDepartmentRepository _deptRepo;
        private readonly IRepository<Staff> _staffRepo;

        public PatientsController(IPatientRepository patientRepo, IDepartmentRepository deptRepo, IRepository<Staff> staffRepo)
        {
            _patientRepo = patientRepo;
            _deptRepo = deptRepo;
            _staffRepo = staffRepo;
        }

        // GET: Patients
        public IActionResult Index()
        {
            var patientList = _patientRepo.GetAll();
            return View(patientList);
        }

        // GET: Patients/Details/5
        public IActionResult Details(int id)
        {
            var patient = _patientRepo.GetById(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            ViewData["dept_id"] = new SelectList(_deptRepo.GetAll(), "Id", "name");
            ViewData["followUp_doctorID"] = new SelectList(_staffRepo.GetAll(), "Id", "name");
            return View();
        }

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                ViewData["dept_id"] = new SelectList(_deptRepo.GetAll(), "Id", "name", patient.dept_id);
                ViewData["followUp_doctorID"] = new SelectList(_staffRepo.GetAll(), "Id", "name", patient.followUp_doctorID);
                return View(patient);
            }

            _patientRepo.Add(patient);
            return RedirectToAction(nameof(Index));
        }

        // GET: Patients/Edit/5
        public IActionResult Edit(int id)
        {
            var patient = _patientRepo.GetById(id);
            if (patient == null) return NotFound();

            ViewData["dept_id"] = new SelectList(_deptRepo.GetAll(), "Id", "name", patient.dept_id);
            ViewData["followUp_doctorID"] = new SelectList(_staffRepo.GetAll(), "Id", "name", patient.followUp_doctorID);
            return View(patient);
        }

        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Patient patient)
        {
            if (id != patient.Id) return NotFound();
            if (!ModelState.IsValid) return View(patient);

            _patientRepo.Update(patient);
            return RedirectToAction(nameof(Index));
        }

        // GET: Patients/Delete/5
        public IActionResult Delete(int id)
        {
            var patient = _patientRepo.GetById(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _patientRepo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}