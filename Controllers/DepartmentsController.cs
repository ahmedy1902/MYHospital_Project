using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CareNet_System.Models;
using CareNet_System.Repository;

namespace CareNet_System.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository _deptRepo;

        public DepartmentsController(IDepartmentRepository deptRepo)
        {
            _deptRepo = deptRepo;
        }

        public IActionResult Index()
        {
            var deptList = _deptRepo.GetAll();
            return View(deptList);
        }

        public IActionResult Details(int id)
        {
            var department = _deptRepo.GetById(id);
            if (department == null) return NotFound();
            return View(department);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department department)
        {
            if (!ModelState.IsValid) return View(department);

            _deptRepo.Add(department);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var department = _deptRepo.GetById(id);
            if (department == null) return NotFound();
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Department department)
        {
            if (id != department.Id) return NotFound();
            if (!ModelState.IsValid) return View(department);

            _deptRepo.Update(department);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var department = _deptRepo.GetById(id);
            if (department == null) return NotFound();
            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _deptRepo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}