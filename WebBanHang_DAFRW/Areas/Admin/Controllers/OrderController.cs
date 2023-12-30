using App.Data;
using DAFW_IS220.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _dataContext;
        public OrderController(UserManager<AppUser> userManager, DataContext context)
        {
            _userManager = userManager;
            _dataContext = context;
        }
        public async Task<IActionResult> Index()
        {
            string userID = "";
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                userID = user.Id;
            }
            ViewBag.User = user.TENKH;
            return View(await _dataContext.Orders.OrderByDescending(p => p.CreateDate).ToListAsync());
        }
        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var orderdetails = await _dataContext.OrderDetails.Include(od=>od.Product).Where(od=>od.OrderCode==ordercode).ToListAsync();
            return View(orderdetails);
        }
    }   
}
