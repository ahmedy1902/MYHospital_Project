using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IPatientRepository _patientsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StaffsController(IRepository<Staff> staffRepository, IPatientRepository PatientRepository, IDepartmentRepository departmentRepository, IWebHostEnvironment webHostEnvironment)
        {
            _staffRepository = staffRepository;
            _departmentRepository = departmentRepository;
            _webHostEnvironment = webHostEnvironment;
            _patientsRepository = PatientRepository;
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

            var staff = _staffRepository.GetAll()
                .FirstOrDefault(s => s.Id == id);

            if (staff == null) return NotFound();

            staff.department = _departmentRepository.GetById(staff.dept_id);

            // Fetch patients only if the staff member is a doctor
            if (staff.title == StaffTitle.Doctor)
            {
                staff.patients = _patientsRepository.GetAll()
                    .Where(p => p.followUp_doctorID == staff.Id)
                    .ToList();
            }

            return View(staff);
        }
        public IActionResult Create()
        {
            PrepareViewBags();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,name,title,salary,seniority_level,experience_years,dept_id")] Staff staff, IFormFile personal_photo)
        {
            if (ModelState.IsValid)
            {
                if (personal_photo != null && personal_photo.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "staff");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + personal_photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    string fileExtension = Path.GetExtension(personal_photo.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("personal_photo", "Please select a valid image file (jpg, jpeg, png, gif, bmp)");
                        PrepareViewBags(staff);
                        return View(staff);
                    }

                    if (personal_photo.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("personal_photo", "Image size must be less than 2MB");
                        PrepareViewBags(staff);
                        return View(staff);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        personal_photo.CopyTo(fileStream);
                    }

                    staff.personal_photo = "/images/staff/" + uniqueFileName;
                }

                _staffRepository.Add(staff);
                _staffRepository.Save();
                return RedirectToAction("Index");
            }

            PrepareViewBags(staff);
            return View(staff);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var staff = _staffRepository.GetAll().FirstOrDefault(s => s.Id == id);
            if (staff == null) return NotFound();

            PrepareViewBags(staff);
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,name,title,salary,seniority_level,experience_years,personal_photo,dept_id")] Staff staff, IFormFile newPhoto)
        {
            if (id != staff.Id) return NotFound();

            if (ModelState.IsValid)
            {
                string oldPhotoPath = staff.personal_photo;

                if (newPhoto != null && newPhoto.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "staff");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + newPhoto.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    string fileExtension = Path.GetExtension(newPhoto.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("newPhoto", "Please select a valid image file (jpg, jpeg, png, gif, bmp)");
                        PrepareViewBags(staff);
                        return View(staff);
                    }

                    if (newPhoto.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("newPhoto", "Image size must be less than 2MB");
                        PrepareViewBags(staff);
                        return View(staff);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        newPhoto.CopyTo(fileStream);
                    }

                    staff.personal_photo = "/images/staff/" + uniqueFileName;

                    if (!string.IsNullOrEmpty(oldPhotoPath) && oldPhotoPath != staff.personal_photo)
                    {
                        string oldPhotoFullPath = Path.Combine(_webHostEnvironment.WebRootPath, oldPhotoPath.TrimStart('/'));
                        if (System.IO.File.Exists(oldPhotoFullPath))
                        {
                            System.IO.File.Delete(oldPhotoFullPath);
                        }
                    }
                }

                _staffRepository.Update(staff);
                _staffRepository.Save();
                return RedirectToAction(nameof(Index));
            }

            PrepareViewBags(staff);
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
            var staff = _staffRepository.GetAll().FirstOrDefault(s => s.Id == id);

            if (staff != null && !string.IsNullOrEmpty(staff.personal_photo))
            {
                string photoPath = Path.Combine(_webHostEnvironment.WebRootPath, staff.personal_photo.TrimStart('/'));
                if (System.IO.File.Exists(photoPath))
                {
                    System.IO.File.Delete(photoPath);
                }
            }

            _staffRepository.Delete(id);
            _staffRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        private void PrepareViewBags(Staff staff = null)
        {
            ViewBag.dept_id = new SelectList(_departmentRepository.GetAll(), "Id", "name", staff?.dept_id);

            ViewBag.TitleList = Enum.GetValues(typeof(StaffTitle))
                .Cast<StaffTitle>()
                .Select(t => new SelectListItem { Value = t.ToString(), Text = t.ToString() })
                .ToList();

            ViewBag.LevelList = Enum.GetValues(typeof(Level))
                .Cast<Level>()
                .Select(l => new SelectListItem { Value = l.ToString(), Text = l.ToString() })
                .ToList();
        }
    }
}