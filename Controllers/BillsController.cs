using Microsoft.AspNetCore.Mvc;
using CareNet_System.Models;
using CareNet_System.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace CareNet_System.Controllers
{
    [Authorize]

    public class BillsController : Controller
    {
        private readonly IBillsRepository _billsRepo;
        private readonly IPatientRepository _patientRepo;

        public BillsController(IBillsRepository billsRepo, IPatientRepository patientRepo)
        {
            _billsRepo = billsRepo;
            _patientRepo = patientRepo;
        }

        // GET: Bills
        public IActionResult Index()
        {
            var billsList = _billsRepo.GetAll();
            return View(billsList);
        }

        // GET: Bills/Details/5
        public IActionResult Details(int id)
        {
            var bill = _billsRepo.GetById(id);
            if (bill == null) return NotFound();
            return View(bill);
        }

        // GET: Bills/Create
        public IActionResult Create()
        {
            ViewData["patient_id"] = new SelectList(_patientRepo.GetAll(), "Id", "name");
            return View();
        }

        // POST: Bills/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Bills bill)
        {
            if (!ModelState.IsValid)
            {
                ViewData["patient_id"] = new SelectList(_patientRepo.GetAll(), "Id", "name", bill.patient_id);
                return View(bill);
            }

            _billsRepo.Add(bill);
            return RedirectToAction(nameof(Index));
        }

        // GET: Bills/Edit/5
        public IActionResult Edit(int id)
        {
            var bill = _billsRepo.GetById(id);
            if (bill == null) return NotFound();

            ViewData["patient_id"] = new SelectList(_patientRepo.GetAll(), "Id", "name", bill.patient_id);
            return View(bill);
        }

        // POST: Bills/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Bills bill)
        {
            if (id != bill.Id) return NotFound();
            if (!ModelState.IsValid) return View(bill);

            _billsRepo.Update(bill);
            return RedirectToAction(nameof(Index));
        }

        // GET: Bills/Delete/5
        public IActionResult Delete(int id)
        {
            var bill = _billsRepo.GetById(id);
            if (bill == null) return NotFound();
            return View(bill);
        }

        // POST: Bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _billsRepo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}