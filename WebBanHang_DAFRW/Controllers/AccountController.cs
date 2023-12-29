//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using System.Security.Principal;
//using WebBanHang_DAFRW.Models;
//using WebBanHang_DAFRW.Models.ViewModels;
//using WebBanHang_DAFRW.Repository;

//namespace WebBanHang_DAFRW.Controllers
//{
//    public class AccountController : Controller
//    {
//        private readonly DataContext _dataContext;
//        private UserManager<AppUserModel> _userManager;
//        private SignInManager<AppUserModel> _signInManager;
//        //private UserManager<CustomerModel> _customerManager;
//        public AccountController(SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManager, /*UserManager<CustomerModel> customerManager,*/ DataContext dataContext)
//        {
//            _dataContext = dataContext;
//            _userManager = userManager;
//            _signInManager = signInManager;
//            //_customerManager = customerManager;
//        }
//        public class CustomerAccount
//        {
//            public CustomerModel? Customer { get; set; }
//            public UserModel? Account { get; set; }
//        }
//        public IActionResult Login(string returnUrl)
//        {
//            return View(new LoginViewModel { ReturnURL = returnUrl });
//        }
//        [HttpPost]
//        public async Task<IActionResult> Login(LoginViewModel loginVM)
//        {
//            if (ModelState.IsValid)
//            {
//                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);
//                if (result.Succeeded)
//                {
//                    return Redirect(loginVM.ReturnURL ?? "/");
//                }
//                ModelState.AddModelError("", "Invalid Username and Password");
//            }
//            return View(loginVM);
//        }
//        public IActionResult Create()
//        {
//            return View();
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(RegisterViewModel user)
//        {
//            if (ModelState.IsValid)
//            {
//                // Tạo tài khoản người dùng
//                AppUserModel newUser = new AppUserModel { UserName = user.Account.UserName };
//                IdentityResult userCreationResult = await _userManager.CreateAsync(newUser, user.Account.Password);

//                if (userCreationResult.Succeeded)
//                {
//                    // Tạo CustomerModel chỉ sau khi tạo User thành công
//                    CustomerModel newCustomer = new CustomerModel
//                    {
//                        AccountId = int.Parse(newUser.Id),
//                        FullName = user.Customer.FullName,
//                        Phone = user.Customer.Phone,
//                        Email = user.Customer.Email
//                    };

//                    _dataContext.Add(newCustomer);
//                    await _dataContext.SaveChangesAsync();

//                    TempData["Success"] = "Create User Success";
//                    return RedirectToAction("Login");
//                }
//                else
//                {
//                    foreach (IdentityError error in userCreationResult.Errors)
//                    {
//                        ModelState.AddModelError(string.Empty, error.Description);
//                    }
//                }
//            }
//            return View(user);
//        }

//        public async Task<IActionResult> Logout()
//        {
//            await _signInManager.SignOutAsync(); // Đăng xuất người dùng

//            return Redirect("/");
//        }
//    }
//}
