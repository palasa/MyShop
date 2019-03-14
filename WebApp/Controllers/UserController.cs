using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        private Entities db = new Entities();


        #region 注册模块
        // 展示表单页面
        [HttpGet]
        public ActionResult Register()
        {
            return View();          
        }

        /// <summary>
        /// 把 数据写入数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(WebApp.ViewModels.User user)
        {
            Models.User u = new Models.User {
                Username = user.Username ,
                Password = Common.JiaMi.Md5 (user.Password ) ,
                Nickname = user.Nickname
            };
            db.User.Add(u);
            db.SaveChanges();
            return Content("");
        }

        // 通过 ajax 验证表单是否正确
        [HttpPost]
        public ActionResult AjaxRegister(WebApp.ViewModels.User user)
        {

            // View() ViewResult            响应一个 html 视图页
            // File() FileContentResult     响应一个文件
            // Json() JsonResult            响应json 字符串 "[ {} , {} , {} ]"
            // Content() ContentResult      响应一个 纯 字符串

            // 判断 验证码是否正确
            if (user.VCode.ToLower() != Session["VCode"].ToString().ToLower())
            {
                return Json(new Common.JsonResult
                {
                    Result = ResultType.Error,
                    ErrorMessage = "验证码错误"
                });
            }

            // 判断 用户名是否已被使用
            int userCount = db.User.Where(u => u.Username == user.Username).Count();
            if ( userCount == 1)
            {
                return Json(new Common.JsonResult
                {
                    Result = ResultType.Error,
                    ErrorMessage = "用户名已被使用"
                });
            }
            
            return Json(new Common.JsonResult
            {
                Result = ResultType.Success,
                ErrorMessage = string.Empty
            });
        }

        /// <summary>
        /// 生成验证码的控制器
        /// </summary>
        /// <returns></returns>
        public ActionResult VerifyCode()
        {
            Common.VerifyCode vCode = new Common.VerifyCode();

            string verifyCodeString = vCode.GetVerifyCode(4);
            Session.Add("VCode", verifyCodeString);

            Image image = vCode.GetVerifyImage(verifyCodeString);

            // ViewResult
            // FileContextResult
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            return File( ms.ToArray() , "image/jpeg");
            //Response.ContentType = "image/jpeg";
            //Response.BinaryWrite(ms.ToArray());
        }
        #endregion


        #region 登录模块

        // 登录页面
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // 执行登录
        [HttpPost]
        public ActionResult Login(User user , string returnUrl)
        {
            user.Password = Common.JiaMi.Md5(user.Password);
            var users = db.User.Where(
                u => u.Username == user.Username &&
                u.Password == user.Password
            );
            var loginResult = users.Count() == 1;

            if( loginResult)
            {
                // 登录成功
                Session.Add("user" , users.FirstOrDefault() );
                if (returnUrl != null && returnUrl != "")
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("List", "Product");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "用户名或密码错误！";
                return View();
            }
        }

        public ActionResult Logout(string returnUrl)
        {
            Session.Remove("user");
            return Redirect(returnUrl);
        }


        #endregion

    }
}