using Ecomm_Project_106.DataAccess.Data;
using Ecomm_Project_106.DataAccess.Repository;
using Ecomm_Project_106.DataAccess.Repository.IRepository;
using Ecomm_Project_106.Models;
using Ecomm_Project_106.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm_Project_106.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork,ApplicationDbContext context)
        {
             _context = context;    
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
            var UserList = _context.ApplicationUsers.ToList();  //aspnetuser
            var RoleList = _context.Roles.ToList();         //aspnetroles
            var UserRoles = _context.UserRoles.ToList();  //aspnetuserroles
            foreach(var user in UserList)
            {
                var roleId = UserRoles.FirstOrDefault(ur=>ur.UserId == user.Id).RoleId;
                user.Role = RoleList.FirstOrDefault(r => r.Id == roleId).Name;
                if(user.CompanyId==null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
                if (user.CompanyId != null)
                {
                    user.Company = new Company()
                    {
                        Name = _unitOfWork.Company.Get(Convert.ToInt32(user.CompanyId)).Name
                    };
                    
                }
            }
            //Remove Admin Role User
            var adminuser = UserList.FirstOrDefault(u => u.Role == SD.Role_Admin);
            if (adminuser != null) UserList.Remove(adminuser);
            return Json(new { data = UserList });
        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            bool isLocked = false;
            var UserInDb = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (UserInDb == null)
                return Json(new { success = false, message = "Something Went Wrong While lock and unlock user!!!" });
            if(UserInDb!=null&&UserInDb.LockoutEnd>DateTime.Now)
            {
                UserInDb.LockoutEnd = DateTime.Now;
                isLocked = false; 
            }
            else
            { 
               UserInDb.LockoutEnd = DateTime.Now.AddYears(100);
                isLocked = true;
            }
            _context.SaveChanges();
            return Json(new { success = true, message = isLocked == true ? "user successfully locked" : "user successfully unlockled" });
                
        }
        #endregion
    }
}
