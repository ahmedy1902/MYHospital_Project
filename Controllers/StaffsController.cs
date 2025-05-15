using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CareNet_System.Models;
using CareNet_System.Repository;

namespace CareNet_System.Controllers
{
    public class StaffsController : Controller
    {
        private readonly IRepository<Staff> _staffRepository;

        public StaffsController(IRepository<Staff> staffRepository)
        {
            _staffRepository = staffRepository;
        }

        // GET: Staffs
        public IActionResult Index()
        {
            var staffList = _staffRepository.GetAll();
            return View(staffList);
        }

        // GET: Staffs/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            var staff = _staffRepository.GetAll().FirstOrDefault(s => s.Id == id);
            if (staff == null) return NotFound();
            return View(staff);
        }

        // GET: Staffs/Create
        public IActionResult Create()
        {
            ViewData["dept_id"] = new SelectList(_staffRepository.GetAll(), "Id", "Id");

            ViewBag.TitleList = Enum.GetValues(typeof(StaffTitle))
                .Cast<StaffTitle>()
                .Select(t => new SelectListItem { Value = t.ToString(), Text = t.ToString() })
                .ToList();

            ViewBag.LevelList = Enum.GetValues(typeof(Level))
                .Cast<Level>()
                .Select(l => new SelectListItem { Value = l.ToString(), Text = l.ToString() })
                .ToList();

            return View();
        }


        // POST: Staffs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create([Bind("Id,name,title,salary,seniority_level,experience_years,personal_photo,dept_id")] Staff staff)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);  // 🔹 طباعة جميع الأخطاء
                }

                return View(staff);  // 🔹 إعادة عرض الصفحة مع عرض الأخطاء
            }

            _staffRepository.Add(staff);
            _staffRepository.Save();
            return RedirectToAction("Index");  // 🔹 العودة إلى قائمة الموظفين بعد الإنشاء
        }
        // GET: Staffs/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var staff = _staffRepository.GetAll().FirstOrDefault(s => s.Id == id);
            if (staff == null) return NotFound();

            // 🔹 تمرير القوائم إلى ViewBag
            ViewData["dept_id"] = new SelectList(_staffRepository.GetAll(), "Id", "Id", staff.dept_id);

            ViewBag.TitleList = Enum.GetValues(typeof(StaffTitle))
                .Cast<StaffTitle>()
                .Select(t => new SelectListItem { Value = t.ToString(), Text = t.ToString() })
                .ToList();

            ViewBag.LevelList = Enum.GetValues(typeof(Level))
                .Cast<Level>()
                .Select(l => new SelectListItem { Value = l.ToString(), Text = l.ToString() })
                .ToList();

            return View(staff);
        }

        // POST: Staffs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,name,title,salary,seniority_level,experience_years,personal_photo,dept_id")] Staff staff)
        {
            if (id != staff.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _staffRepository.Update(staff);
                _staffRepository.Save();
                return RedirectToAction(nameof(Index));
            }

            // 🔹 إعادة تمرير القوائم إذا فشل التحقق
            ViewData["dept_id"] = new SelectList(_staffRepository.GetAll(), "Id", "Id", staff.dept_id);

            ViewBag.TitleList = Enum.GetValues(typeof(StaffTitle))
                .Cast<StaffTitle>()
                .Select(t => new SelectListItem { Value = t.ToString(), Text = t.ToString() })
                .ToList();

            ViewBag.LevelList = Enum.GetValues(typeof(Level))
                .Cast<Level>()
                .Select(l => new SelectListItem { Value = l.ToString(), Text = l.ToString() })
                .ToList();

            return View(staff);
        }
        // GET: Staffs/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var staff = _staffRepository.GetAll().FirstOrDefault(s => s.Id == id);
            if (staff == null) return NotFound();
            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _staffRepository.Delete(id);
            _staffRepository.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}