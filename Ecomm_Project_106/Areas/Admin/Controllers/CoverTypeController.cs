using Dapper;
using Ecomm_Project_106.DataAccess.Repository;
using Ecomm_Project_106.DataAccess.Repository.IRepository;
using Ecomm_Project_106.Models;
using Ecomm_Project_106.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm_Project_106.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin+","+SD.Role_Employee)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.SP_CALL.List<CoverType>(SD.GetCoverTypes) });
            //return Json(new { data = _unitOfWork.CoverType.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //var CoverTypeInDb = _unitOfWork.CoverType.Get(id);
            DynamicParameters param = new DynamicParameters();
            param.Add("id", id);
            var CoverTypeInDb = _unitOfWork.SP_CALL.OneRecord<CoverType>
                (SD.GetCoverType, param);
            if(CoverTypeInDb==null)
                return Json(new {success=false,
                    message="something went wrong while delete data!!!"
                });
            _unitOfWork.SP_CALL.Execute(SD.DeleteCoverType, param);
            //_unitOfWork.CoverType.Remove(CoverTypeInDb);
            //_unitOfWork.Save();
            return Json(new { success = true, message = "Data Delete Successfully!" });
        }
        #endregion
        public IActionResult Upsert(int?id)
        {
            CoverType coverType = new CoverType();
            if (id == null) return View(coverType);
            DynamicParameters param = new DynamicParameters();
            param.Add("id", id.GetValueOrDefault());
            coverType = _unitOfWork.SP_CALL.OneRecord<CoverType>(SD.GetCoverType,param);
            //coverType = _unitOfWork.CoverType.Get(id.GetValueOrDefault());
            if (coverType == null) return NotFound();
            return View(coverType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (coverType == null) return NotFound();
            if (!ModelState.IsValid) return View(coverType);
            DynamicParameters param = new DynamicParameters();
            param.Add("name", coverType.Name);
            if (coverType.Id == 0)
                //_unitOfWork.CoverType.Add(coverType);
                _unitOfWork.SP_CALL.Execute(SD.CreateCoverType, param);
            else
            {
                param.Add("id", coverType.Id);
                _unitOfWork.SP_CALL.Execute(SD.updatecoverType, param);
            }
                //    _unitOfWork.CoverType.Update(coverType);
                //_unitOfWork.Save();
                return RedirectToAction(nameof(Index));
        }
            
    }
}
