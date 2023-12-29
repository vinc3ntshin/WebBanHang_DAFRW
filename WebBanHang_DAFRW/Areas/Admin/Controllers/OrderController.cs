using App.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang_DAFRW.Repository;

namespace WebBanHang_DAFRW.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleName.Administrator)]
    [Area("Admin")]
    //[Authorize]
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;
        public OrderController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Orders.OrderByDescending(p => p.CreateDate).ToListAsync());
        }
        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var orderdetails = await _dataContext.OrderDetails.Include(od=>od.Product).Where(od=>od.OrderCode==ordercode).ToListAsync();
            return View(orderdetails);
        }
    }   
}
