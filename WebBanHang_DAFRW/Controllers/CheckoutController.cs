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
using App.DBModels;
using Bogus;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.X9;
using System.Configuration;
using WebBanHang_DAFRW.Areas.Admin.Controllers;
using DAFWUtils = DAFW_IS220.Models.Utils;

// Bây giờ bạn có thể sử dụng DAFWUtils để tham chiếu đến DAFW_IS220.Models.Utils

namespace WebBanHang_DAFRW.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _dataContext;

        private readonly IConfiguration configuration;

        private readonly ILogger<CheckoutController> _logger;

        private readonly IHttpContextAccessor httpContext;
        public CheckoutController(DataContext context, UserManager<AppUser> userManager, IConfiguration configuration, ILogger<CheckoutController> logger, IHttpContextAccessor http
            )
        {
            _userManager = userManager;
            _dataContext = context;
            this.configuration = configuration;
            this._logger = logger;
            httpContext = http;
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
            var code = new { success = false, Code = -1, Url = "" };
            string CustomerName = "";
            string Phone = "";
            string Address = "";
            string Email = "";
            string userID = "";
            var user = await _userManager.GetUserAsync(User);
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
            var orderItem = new OrderModel
            {
                OrderCode = ordercode,
                UserId = user.Id,
                TenKH = user.TENKH,
                Status = 1,
                TotalMoney = total,
                CreateDate = DateTime.Now
            };
            _dataContext.Add(orderItem);
            _dataContext.SaveChanges();
            foreach (var cart in cartItems)
            {
                var orderdetails = new OrderDetails
                {
                    OrderCode = ordercode,
                    ProductId = cart.ProductId,
                    Price = cart.Price,
                    Quantity = cart.Quantity
                };
                _dataContext.Add(orderdetails);
                await _dataContext.SaveChangesAsync();
            }
            HttpContext.Session.Remove("Cart");
            TempData["success"] = "Checkout thành công";
            var url = UrlPayment(0, ordercode);
            // code = new { success = true, Code = orderModel.TypePayment, Url = url };
            //return Ok(new { success = true, code = 2, url = url });
            return Redirect(url);

            //}
        }

        public async Task<IActionResult> AddAddress([FromForm] string address)
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

        public async Task<ActionResult> VnpayReturn()
        {
            if (Request.Query.Keys.Count > 0)
            {
                var vnpConfig = configuration.GetSection("VNPAY_SETTINGS");
                string vnp_HashSecret = vnpConfig["vnp_HashSecret"] ?? "";
                var vnpayData = Request.Query.Keys;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string key in vnpayData)
                {
                    // Lấy tất cả dữ liệu chuỗi truy vấn
                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(key, Request.Query[key]);
                    }
                }
                int orderCode = Convert.ToInt32(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.Query["vnp_SecureHash"];
                String TerminalID = Request.Query["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.Query["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        // var itemOrder = db.Orders.FirstOrDefault(x => x.Code == orderCode);

                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        ViewBag.PaymentStatus = "Đã thanh toán";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        ViewBag.Result = 0;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
            }
            //var a = UrlPayment(0, "DH3574");
            return View();
        }

        #region Thanh toán vnpay
        public string UrlPayment(int TypePaymentVN, string orderCode)
        {
            var urlPayment = "";
            //var sorder = db.Orders.FirstOrDefault(x => x.Code == orderCode);
            Random random = new Random();
            int orderId = random.Next();
            var donhang = _dataContext.Orders.FirstOrDefault(o => o.OrderCode.Equals(orderCode));
            OrderInfo order = new OrderInfo();
            order.OrderId = orderId;
            order.Amount = donhang != null ? (long)donhang.TotalMoney * 100 : 0;
            order.Status = "0";
            order.CreatedDate = DateTime.Now;
            var vnpConfig = configuration.GetSection("VNPAY_SETTINGS");
            //Get Config Info
            // string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            // string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            // string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            // string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key
            string vnp_Returnurl = vnpConfig["vnp_Returnurl"] ?? "";
            string vnp_Url = vnpConfig["vnp_Url"] ?? "";
            string vnp_TmnCode = vnpConfig["vnp_TmnCode"] ?? "";
            string vnp_HashSecret = vnpConfig["vnp_HashSecret"] ?? "";
            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)donhang.TotalMoney * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", order.Amount.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", new DAFWUtils(httpContext).GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            _logger.LogInformation("VNPAY URL: {0}", urlPayment);
            return urlPayment;
        }
        #endregion

    }
}
