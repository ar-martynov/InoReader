using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;

using Domain.Models;
using Domain.Concrete;
using Domain.Abstract;
using Domain.Context;
using Domain.Entities;

namespace InoReader.Controllers
{
    public class AccountController : Controller
    {
        private IUnitOfWork unitOfWork;
        public AccountController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        #region Login Methods
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated) return RedirectToAction("Index", "Home");
            return PartialView();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel user)
        {
            if (Request.IsAuthenticated) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var repository = unitOfWork.UsersRepository;
                User newUser = Mapper.Map<LoginModel, User>(user);

                newUser = repository.GetUser(newUser);

                if (newUser != null)
                {
                    try
                    {
                        SetCookie(newUser.UserName, newUser.UserId);
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("Error", e.Message);
                    }

                    return Json(new { success = true });
                }

                ModelState.AddModelError("UserNotFound", "Неверный логин или пароль");
                return PartialView("Login", user);
            }

            return PartialView("Login", user);
        }
        #endregion

        #region Registration Methods
        //вызывается при попытке зарегистрироваться
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Registration()
        {
            if (Request.IsAuthenticated) return RedirectToAction("Index", "Home");

            return PartialView("RegistrationForm");
        }

        //принимает регистрационные даннные
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Registration(RegistrationModel user)
        {
            if (Request.IsAuthenticated) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                User newUser = Mapper.Map<RegistrationModel, User>(user);
                var repository = unitOfWork.UsersRepository;

                try
                {
                    if (repository.IsUserValid(newUser))
                    {
                        //создаем нового пользователя
                        repository.Registration(ref newUser);
                        unitOfWork.Commit();

                        if (newUser != null)
                            SetCookie(newUser.UserName, newUser.UserId);

                        return Json(new { success = true });
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Error", e.Message);
                    return PartialView("RegistrationForm", user);
                }
            }
            return PartialView("RegistrationForm", user);
        }
        #endregion

        [Authorize]
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private void SetCookie(string login, int userId)
        {
            try
            {
                var ticket = new FormsAuthenticationTicket(
                    1,
                    login,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    false,
                    userId.ToString()
                );
                Response.Cookies.Add
                (
                    new HttpCookie
                    (
                        FormsAuthentication.FormsCookieName,
                        FormsAuthentication.Encrypt(ticket)
                    )
                );
            }
            catch
            {

            }
        }
    }
}
