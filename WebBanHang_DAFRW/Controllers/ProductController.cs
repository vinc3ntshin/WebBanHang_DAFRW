using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang_DAFRW.Models;
using WebBanHang_DAFRW.Repository;

namespace WebBanHang_DAFRW.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        public ProductController(DataContext context)
        {
            _dataContext = context;
        }
        public IActionResult Index()
        {
            List<ProductModel> products = _dataContext.Products.Include("Category").Include("Brand").ToList();
            return View(products);
        }
        public async Task<IActionResult> Details(int Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }

            var productsById = _dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();
            return View(productsById);
        }
    }
}
