using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHang_DAFRW.Models;
using WebBanHang_DAFRW.Repository;
using System.Runtime.CompilerServices;
using DAFW_IS220.Models;
using Microsoft.AspNetCore.Identity;
using App.Data;

namespace WebBanHang_DAFRW.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleName.Administrator)]
    [Area("Admin")]
    public class AcountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _dataContext;
        public AcountController(DataContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _dataContext = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }
    }
}
