using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class OrderController : Controller
    {
        private Entities db = new Entities();

        // GET: Order
        public ActionResult Add(int AddressId , int?[] ProductId , int?[] Number)
        {
            User user = (Session["user"] as User);

            // 设置新增的订单
            Order order = new Order
            {
                UserId = user.Id,
                OrderTime = DateTime.Now,
                AddressId = AddressId,
                Price = 1500,
                OrderState = 0
            };
            db.Order.Add(order);

            // 设置该订单的详情
            for (int i=0;i<ProductId.Length;i++)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    Order = order,
                    ProductId = ProductId[i],
                    Number = Number[i]
                };
                db.OrderDetail.Add(orderDetail);
            }

            // 从我的购物车中删除对应的商品

            IQueryable<int?> productIdList = ProductId.AsQueryable();
            var cartToDelete = db.Cart.Where(c => c.UserId == user.Id && productIdList.Contains(c.ProductId));
            db.Cart.RemoveRange(cartToDelete);

            db.SaveChanges();

            return RedirectToAction("Detail/" + order.Id , "Order");
        }
    }
}