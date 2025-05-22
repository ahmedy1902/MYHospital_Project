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
        public IActionResult Index(string paymentFilter, int? patientFilter)
        {
            var billsList = _billsRepo.GetAll();

            if (!string.IsNullOrEmpty(paymentFilter) &&
                Enum.TryParse<billMethod>(paymentFilter, out var method))
            {
                billsList = billsList.Where(b => b.Payment_Method == method).ToList();
            }

            if (patientFilter.HasValue)
            {
                billsList = billsList.Where(b => b.patient_id == patientFilter.Value).ToList();
            }

            ViewBag.PaymentMethods = new SelectList(
                Enum.GetValues(typeof(billMethod)).Cast<billMethod>(), paymentFilter);

            ViewBag.PatientList = new SelectList(
                _patientRepo.GetAll(), "Id", "name", patientFilter);

            ViewBag.SelectedPayment = paymentFilter;
            ViewBag.SelectedPatient = patientFilter;

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
            ViewBag.PaymentMethods = new SelectList(
                Enum.GetValues(typeof(billMethod))
                    .Cast<billMethod>()
                    .Select(m => new {
                        Value = m.ToString(),
                        Text = m.ToString()
                    }),
                "Value",
                "Text"
            );

            ViewBag.PatientList = new SelectList(
                _patientRepo.GetAll().Select(p => new {
                    p.Id,
                    FullName = $"{p.name} (ID: {p.Id})"
                }),
                "Id",
                "FullName"
            );

            return View();
        }

        // POST: Bills/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Bills bill)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PaymentMethods = new SelectList(
                    Enum.GetValues(typeof(billMethod))
                        .Cast<billMethod>()
                        .Select(m => new {
                            Value = m.ToString(),
                            Text = m.ToString()
                        }),
                    "Value",
                    "Text",
                    bill.Payment_Method.ToString()
                );

                ViewBag.PatientList = new SelectList(
                    _patientRepo.GetAll().Select(p => new {
                        p.Id,
                        FullName = $"{p.name} (ID: {p.Id})"
                    }),
                    "Id",
                    "FullName",
                    bill.patient_id
                );

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

            // قائمة المرضى
            ViewData["patient_id"] = new SelectList(_patientRepo.GetAll(), "Id", "name", bill.patient_id);

            // تحويل enum billMethod إلى قائمة SelectListItems
            var paymentMethods = Enum.GetValues(typeof(billMethod))
                                     .Cast<billMethod>()
                                     .Select(pm => new SelectListItem
                                     {
                                         Text = pm.ToString(),
                                         Value = pm.ToString(),
                                         Selected = (pm == bill.Payment_Method)
                                     }).ToList();

            ViewData["Payment_Method"] = paymentMethods;

            return View(bill);
        }

        // POST: Bills/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Bills bill)
        {
            if (id != bill.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["patient_id"] = new SelectList(_patientRepo.GetAll(), "Id", "name", bill.patient_id);

                var paymentMethods = Enum.GetValues(typeof(billMethod))
                                         .Cast<billMethod>()
                                         .Select(pm => new SelectListItem
                                         {
                                             Text = pm.ToString(),
                                             Value = pm.ToString(),
                                             Selected = (pm == bill.Payment_Method)
                                         }).ToList();

                ViewData["Payment_Method"] = paymentMethods;

                return View(bill);
            }

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