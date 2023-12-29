using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebBanHang_DAFRW.Models;
using WebBanHang_DAFRW.Repository;
using System.Security.Claims;
using DAFW_IS220.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHang_DAFRW.Models.ViewModels;

namespace WebBanHang_DAFRW.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _dataContext;
        public CheckoutController(DataContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _dataContext = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userID = "null";
            if (user != null)
            {
                userID = user.Id;
                ViewBag.User = user;
            }
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            var cartItemList = cartItems.Where(item => item.UserId.Equals(userID)).ToList();
            var total = cartItemList.Sum(item => item.Total);
            CartItemViewModel cartVM = new()
            {
                CartItems = cartItemList,
                GrandTotal = total
            };

            return View(cartVM);
        }

        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            var userID = "null";
            if (user != null)
            {
                userID = user.Id;
            }
            var userName = user.TENKH;
            //if (user.Id == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            //else
            //{
            var users = await _userManager.Users.ToListAsync();
            ViewBag.User = user.TENKH;
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                decimal total = cartItems.Sum(item => item.Quantity * item.Price);
                var ordercode = Guid.NewGuid().ToString();
                var orderItem = new OrderModel();
                orderItem.OrderCode = ordercode;
                orderItem.UserId = user.Id;
                orderItem.Status = 1;
                orderItem.TotalMoney = total;
                orderItem.CreateDate = DateTime.Now;
                _dataContext.Add(orderItem);
                _dataContext.SaveChanges();
                foreach (var cart in cartItems)
                {
                    var orderdetails = new OrderDetails();
                    orderdetails.OrderCode = ordercode;
                    orderdetails.ProductId = cart.ProductId;
                    orderdetails.Price = cart.Price;
                    orderdetails.Quantity = cart.Quantity;
                    _dataContext.Add(orderdetails);
                    await _dataContext.SaveChangesAsync();
                }
                HttpContext.Session.Remove("Cart");
                TempData["success"] = "Checkout thành công";
                return RedirectToAction("Index", "Cart");
            //}
        }

        public async Task<IActionResult> AddAddress([FromForm]string address)
        {
            var user = await _userManager.GetUserAsync(User);
            if (address != null && user != null)
            {
                user.DIACHI = address;
                _dataContext.Update(user);
                await _dataContext.SaveChangesAsync();
                return Ok();
            }
            else { return NotFound(); }
        }

    }
}
