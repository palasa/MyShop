using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CartController : Controller
    {

        private Entities db = new Entities();

        // GET: Cart
        public ActionResult Add(int ProductId)
        {
            if ( Session["user"] == null )
            {
                // 未登录
                return Redirect("~/User/Login");
            }

            User currentUser = (Session["user"] as User);

            // 判断当前用户的购物车中有没有这件商品
            var result = db.Cart.Where(cart => cart.UserId == currentUser.Id && cart.ProductId == ProductId);

            // 如果已有， 数量加一
            if ( result.Count() == 1)
            {
                Cart currentCart = result.FirstOrDefault();
                currentCart.Number++;
            }
            else
            {
                // 如果没有，新增一条
                // 把该商品加入购物车的表中去
                Cart c = new Cart
                {
                    // User = Session["user"] as User ,
                    // Product = db.Product.Find(ProductId) ,
                    UserId = currentUser.Id,
                    ProductId = ProductId,
                    Number = 1
                };
                db.Cart.Add(c);
            }
            
            db.SaveChanges();

            return View("List");
        }

        public ActionResult List()
        {
            if( Session["user"] == null )
            {
                return RedirectToAction("Login", "User");
            }
            
            // 获取当前登录用户的购物车中的所有商品
            User user = Session["user"] as User;

            // 读取该用户的所有地址
            ViewBag.Address = db.Address.Where(a => a.UserId == user.Id).OrderBy( a=> a.Id ).ToList();

            // 读取所有的一级地区
            ViewBag.Areas = db.Area.Where(a => a.ParentId == null).ToList();


            return View(db.Cart.Where(cart => cart.UserId == user.Id).ToList());
        }
    }
}