using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MWR.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MWR.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public static string GetSHA1(String texto)  {
            SHA1 sha1 = SHA1CryptoServiceProvider.Create();
            Byte[] textOriginal = ASCIIEncoding.Default.GetBytes(texto);
            Byte[] hash = sha1.ComputeHash(textOriginal);
            StringBuilder cadena = new StringBuilder();
            
            foreach (byte i in hash)
                cadena.AppendFormat("{0:x2}", i);

            return cadena.ToString();
        }

        public AccountController()  { }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )  {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager  {
            get  {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set  { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager  {
            get  {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set  {
                _userManager = value;
            }
        }


        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)  {
            if(User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Report");

            return View();
        }


        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)  {
            if (ModelState.IsValid)  {
                siixsem_main_dbEntities db = new siixsem_main_dbEntities();
                String pass = GetSHA1(model.Password);
                validate_user_Result res = null;

                res = db.validate_user(model.Email, pass).First();

                if (res.RESULT == 1)  {
                    
                    var resL1 = await SignInManager.PasswordSignInAsync(model.Email, pass, false, shouldLockout: false);

                    if (SignInStatus.Success != resL1)  {
                        await UserManager.CreateAsync(new ApplicationUser { UserName = model.Email, Email = model.Email }, pass);
                        await SignInManager.PasswordSignInAsync(model.Email, pass, false, shouldLockout: false);
                    }

                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    Session["User"] = (validate_user_Result)res;

                    return RedirectToAction("Index","Report");

                }  else  { 
                    ModelState.AddModelError("", "Attempt to login was not valid.");
                    return View(model);
                }
            } else  {
                return View(model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()  {
            //Removes session.
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            //Ends local session.
            FormsAuthentication.SignOut();
            AuthenticationManager.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Aplicaciones auxiliares
        // Se usa para la protección XSRF al agregar inicios de sesión externos
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}