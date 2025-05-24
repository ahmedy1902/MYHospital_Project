using Microsoft.AspNetCore.Mvc;
using CareNet_System.Models;
using CareNet_System.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace CareNet_System.Controllers
{
    [Authorize]
    public class PatientsController : Controller
    {
        private readonly IPatientRepository _patientRepo;
        private readonly IDepartmentRepository _deptRepo;
        private readonly IStaffRepository _staffRepo;

        public PatientsController(IPatientRepository patientRepo, IDepartmentRepository deptRepo, IStaffRepository staffRepo)
        {
            _patientRepo = patientRepo;
            _deptRepo = deptRepo;
            _staffRepo = staffRepo;
        }

        // GET: Patients
        public IActionResult Index(int? roomFilter, int? doctorFilter, TreatmentType? treatmentFilter)
        {
            var patients = _patientRepo.GetAll();

            if (roomFilter.HasValue)
            {
                patients = patients.Where(p => p.room_num == roomFilter.Value).ToList();
            }

            if (doctorFilter.HasValue)
            {
                patients = patients.Where(p => p.followUp_doctorID == doctorFilter.Value).ToList();
            }

            if (treatmentFilter.HasValue)
            {
                patients = patients.Where(p => p.treatment == treatmentFilter.Value).ToList();
            }

            ViewBag.RoomList = _patientRepo.GetAll()
                .Where(p => p.room_num != 0)
                .Select(p => new SelectListItem { Value = p.room_num.ToString(), Text = p.room_num.ToString() })
                .Distinct()
                .OrderBy(x => x.Text)
                .ToList();

            ViewBag.DoctorList = _staffRepo.GetAll()
                .Where(s => s.title == StaffTitle.Doctor)
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.name })
                .ToList();

            ViewBag.TreatmentList = Enum.GetValues(typeof(TreatmentType))
                .Cast<TreatmentType>()
                .Select(t => new SelectListItem { Value = t.ToString(), Text = t.ToString() })
                .ToList();

            ViewBag.SelectedRoom = roomFilter?.ToString();
            ViewBag.SelectedDoctor = doctorFilter?.ToString();
            ViewBag.SelectedTreatment = treatmentFilter?.ToString();

            return View(patients);
        }


        // GET: Patients/Details/5
        public IActionResult Details(int id)
        {
            var patient = _patientRepo.GetAll()
                .FirstOrDefault(p => p.Id == id);

            if (patient != null)
            {
                patient.followUpDoctor = _staffRepo.GetById(patient.followUp_doctorID);
            }
            if (patient == null) return NotFound();

            patient.department = _deptRepo.GetById(patient.dept_id);
            patient.followUpDoctor = _staffRepo.GetById(patient.followUp_doctorID);

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            ViewBag.DepartmentList = new SelectList(_deptRepo.GetAll(), "Id", "name");
            ViewBag.StaffList = new SelectList(_staffRepo.GetAll().Where(s => s.title == StaffTitle.Doctor), "Id", "name");

            ViewBag.TreatmentList = new SelectList(
                Enum.GetValues(typeof(TreatmentType))
                    .Cast<TreatmentType>()
                    .Select(t => new { Value = t.ToString(), Text = t.ToString() }),
                "Value",
                "Text"
            );

            return View();
        }

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DepartmentList = new SelectList(_deptRepo.GetAll(), "Id", "name", patient.dept_id);
                ViewBag.StaffList = new SelectList(_staffRepo.GetAll().Where(s => s.title == StaffTitle.Doctor), "Id", "name", patient.followUp_doctorID);

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

            ViewBag.DepartmentList = new SelectList(_deptRepo.GetAll(), "Id", "name", patient.dept_id);
            ViewBag.StaffList = new SelectList(_staffRepo.GetAll().Where(s => s.title == StaffTitle.Doctor), "Id", "name", patient.followUp_doctorID);

            ViewBag.TreatmentList = Enum.GetValues(typeof(TreatmentType))
                .Cast<TreatmentType>()
                .Select(t => new SelectListItem
                {
                    Value = t.ToString(),
                    Text = t.ToString(),
                    Selected = (patient.treatment.HasValue && patient.treatment.Value == t)
                })
                .ToList();

            return View(patient);
        }


        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Patient patient)
        {
            if (id != patient.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.DepartmentList = new SelectList(_deptRepo.GetAll(), "Id", "name", patient.dept_id);
                ViewBag.StaffList = new SelectList(_staffRepo.GetAll().Where(s => s.title == StaffTitle.Doctor), "Id", "name", patient.followUp_doctorID);
                ViewBag.TreatmentList = Enum.GetValues(typeof(TreatmentType))
                    .Cast<TreatmentType>()
                    .Select(t => new SelectListItem
                    {
                        Value = t.ToString(),
                        Text = t.ToString(),
                        Selected = (patient.treatment.HasValue && patient.treatment.Value == t)
                    })
                    .ToList();

                return View(patient);
            }

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