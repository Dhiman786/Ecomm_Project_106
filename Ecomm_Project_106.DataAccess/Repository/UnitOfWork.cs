using Ecomm_Project_106.DataAccess.Data;
using Ecomm_Project_106.DataAccess.Repository.IRepository;
using Ecomm_Project_106.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_Project_106.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
            CoverType = new CoverTypeRepository(context);
            Product=new ProductRepository(context);
            SP_CALL = new SP_CALL(context);
            Company = new CompanyRepository(context);
            ShoppingCart=new ShoppingCartRepository(context);
            OrderDetail=new OrderDetailRepository(context);
            OrderHeader=new OrderHeaderRepository(context);
            ApplicationUser = new UserRepository(context);

        }

        public ICategoryRepository Category { private set; get; }

        public ICoverTypeRepository CoverType { private set; get; }
        public IProductRepository Product { private set; get; }
        public ISP_CALL SP_CALL { private set; get; }
        public ICompanyRepository Company { private set; get; }
        public IShoppingCartRepository ShoppingCart { private set; get; }
        public IOrderHeaderRepository OrderHeader { private set; get; }
        public IOrderDetailRepository OrderDetail { private set; get; }
        public IUserRepository ApplicationUser { private set; get; }


        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
