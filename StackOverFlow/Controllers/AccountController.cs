using StackOverFlow.ServiceLayer;
using StackOverFlow.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverFlow.Controllers
{
    public class AccountController : Controller
    {
        IUsersService usersService;

        public AccountController(IUsersService usersService)
        {
            this.usersService = usersService;
        }
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                int lastestUserID = this.usersService.InsertUser(registerViewModel);
                Session["CurrentUserID"] = lastestUserID;
                Session["CurrentUserName"] = registerViewModel.Name;
                Session["CurrentUserEmail"] = registerViewModel.Email;
                Session["CurrentUserPassword"] = registerViewModel.Password;
                Session["CurrentUserIsAdmin"] = false;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data");
                return View();
            }
        }

        public ActionResult Login()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                UserViewModel existUser = this.usersService.GetUsersByEmailAndPassword(loginViewModel.Email, loginViewModel.Password);
                if (existUser != null)
                {
                    Session["CurrentUserID"] = existUser.UserID;
                    Session["CurrentUserName"] = existUser.Name;
                    Session["CurrentUserEmail"] = existUser.Email;
                    Session["CurrentUserPassword"] = existUser.PasswordHash;
                    Session["CurrentUsersAdmin"] = existUser.IsAdmin;

                    if (existUser.IsAdmin)
                    {
                        return RedirectToRoute(new { area = "Admin", controller = "AdminHome", action = "Index" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("x", "Invalid Email / Password");
                    return View(loginViewModel);
                }
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
            }
            return View(loginViewModel);
        }


        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index","Home");
        }
        

        public ActionResult ChangeProfile()
        {
            int UserID = Convert.ToInt32(Session["CurrentUserID"]);
            UserViewModel updateUser = this.usersService.GetUsersByUserID(UserID);
            EditUserDetailsViewModel editUserDetailsViewModel = new EditUserDetailsViewModel()
            {
                UserID = updateUser.UserID,
                Email = updateUser.Email,
                Mobile = updateUser.Mobile,
                Name = updateUser.Name
            };
            return View(editUserDetailsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProfile(EditUserDetailsViewModel editUserDetailsViewModel)
        {
           
            if (ModelState.IsValid)
            {
                editUserDetailsViewModel.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                this.usersService.UpdateUserDetails(editUserDetailsViewModel);
                Session["CurrentUserName"] = editUserDetailsViewModel.Name;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                return View(editUserDetailsViewModel);
            }
        }

        public ActionResult ChangePassword()
        {
            int UserID = Convert.ToInt32(Session["CurrentUserID"]);
            UserViewModel updateUser = this.usersService.GetUsersByUserID(UserID);
            EditUserPasswordViewModel editUserPasswordViewModel = new EditUserPasswordViewModel()
            {
                UserID = updateUser.UserID,
                Email = updateUser.Email,
                Password = "",
                ConfirmPassword = ""
            };
            return View(editUserPasswordViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(EditUserPasswordViewModel editUserPasswordViewModel)
        {

            if (ModelState.IsValid)
            {
                editUserPasswordViewModel.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                this.usersService.UpdateUserPassword(editUserPasswordViewModel);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                return View(editUserPasswordViewModel);
            }
        }
    }
}