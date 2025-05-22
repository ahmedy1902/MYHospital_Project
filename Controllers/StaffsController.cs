using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CareNet_System.Models;
using CareNet_System.Repository;
using Microsoft.AspNetCore.Authorization;

namespace CareNet_System.Controllers
{
    [Authorize]
    public class StaffsController : Controller
    {
        private readonly IRepository<Staff> _staffRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public StaffsController(IRepository<Staff> staffRepository, IDepartmentRepository departmentRepository)
        {
            _staffRepository = staffRepository;
            _departmentRepository = departmentRepository;
        }

        public IActionResult Index(string titleFilter, int? departmentFilter)
        {
            var staffList = _staffRepository.GetAll();

            ViewBag.TitleList = new SelectList(
                Enum.GetValues(typeof(StaffTitle))
                    .Cast<StaffTitle>()
                    .Select(t => new { Value = t.ToString(), Text = t.ToString() }),
                "Value",
                "Text",
                titleFilter
            );

            ViewBag.DepartmentList = new SelectList(
                _departmentRepository.GetAll(),
                "Id",
                "name",
                departmentFilter
            );

            if (!string.IsNullOrEmpty(titleFilter))
            {
                staffList = staffList.Where(s => s.title.ToString() == titleFilter).ToList();
            }

            if (departmentFilter.HasValue)
            {
                staffList = staffList.Where(s => s.dept_id == departmentFilter.Value).ToList();
            }

            return View(staffList);
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var staff = _staffRepository.GetAll().FirstOrDefault(s => s.Id == id);
            if (staff == null) return NotFound();

            staff.department = _departmentRepository.GetById(staff.dept_id);

            return View(staff);
        }

        public IActionResult Create()
        {
            ViewBag.dept_id = new SelectList(_departmentRepository.GetAll(), "Id", "name");

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,name,title,salary,seniority_level,experience_years,personal_photo,dept_id")] Staff staff)
        {
            if (!ModelState.IsValid)
            {
                return View(staff);
            }

            _staffRepository.Add(staff);
            _staffRepository.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var staff = _staffRepository.GetAll().FirstOrDefault(s => s.Id == id);
            if (staff == null) return NotFound();

            ViewBag.TitleList = Enum.GetValues(typeof(StaffTitle))
                .Cast<StaffTitle>()
                .Select(t => new SelectListItem { Value = t.ToString(), Text = t.ToString() })
                .ToList();

            ViewBag.LevelList = Enum.GetValues(typeof(Level))
                .Cast<Level>()
                .Select(l => new SelectListItem { Value = l.ToString(), Text = l.ToString() })
                .ToList();

            ViewBag.dept_id = new SelectList(_departmentRepository.GetAll(), "Id", "name", staff.dept_id);

            return View(staff);
        }

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

            ViewBag.TitleList = Enum.GetValues(typeof(StaffTitle))
                .Cast<StaffTitle>()
                .Select(t => new SelectListItem { Value = t.ToString(), Text = t.ToString() })
                .ToList();

            ViewBag.LevelList = Enum.GetValues(typeof(Level))
                .Cast<Level>()
                .Select(l => new SelectListItem { Value = l.ToString(), Text = l.ToString() })
                .ToList();

            ViewBag.dept_id = new SelectList(_departmentRepository.GetAll(), "Id", "name", staff.dept_id);

            return View(staff);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var staff = _staffRepository.GetAll().FirstOrDefault(s => s.Id == id);
            if (staff == null) return NotFound();

            staff.department = _departmentRepository.GetById(staff.dept_id); 

            return View(staff);
        }

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