using Ecomm_Project_106.DataAccess.Repository.IRepository;
using Ecomm_Project_106.Models;
using Ecomm_Project_106.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm_Project_106.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin+","+SD.Role_Employee)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var CategoryList = _UnitOfWork.Category.GetAll();
            return Json(new { data = CategoryList });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var CategoryInDb = _UnitOfWork.Category.Get(id);
            if (CategoryInDb == null)
                return Json(new { success = false, message = "Something Went Wrong While Delete Data!!" });
            _UnitOfWork.Category.Remove(CategoryInDb);
            _UnitOfWork.Save();
            return Json(new { success = true, message = "Data Successfully Deleted" });
        }
        #endregion
        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null) return View(category); //create
            //Edit
            category = _UnitOfWork.Category.Get(id.GetValueOrDefault());
            if (category == null) return NotFound();
            return View(category);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (category == null) return NotFound();
            if (!ModelState.IsValid) return View(category);
            if (category.Id == 0)
                _UnitOfWork.Category.Add(category);
            else
                _UnitOfWork.Category.Update(category);
                _UnitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
