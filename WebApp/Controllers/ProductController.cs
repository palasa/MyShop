using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {

        private Entities db = new Entities();

        private const int PAGE_SIZE = 3;

        /// <summary>
        /// 商品列表页
        /// </summary>
        /// <param name="ProductTypeId">商品类别编号</param>
        /// <param name="Order">商品排序方式</param>
        /// <param name="Page">商品当前页页码</param>
        /// <param name="keyword">搜索使用的关键字</param>
        /// <returns></returns>
        public ActionResult List( int? ProductTypeId , int? Order , int? Page , string keyword )
        {
            // 获取商品的一级类别
            ViewBag.ProductTypes = db.ProductType.Where(pt => pt.Pid == null);

            // 所有 【已上架】的 商品数据
            IEnumerable<Product> list = db.Product.Where(p=>p.OnSale==true).AsEnumerable();

            // 处理搜索功能：
            if( keyword!=null && keyword!="" )
            {
                // 执行搜索
                list = list.Where(p => p.Name.Contains(keyword));
            }


            // 如果请求中带有 商品类别的参数
            if (ProductTypeId != null)
            {
                // 获取 该类别下的商品列表
                list = list.Where(p => p.TypeId == ProductTypeId);

                // 获取该类别的对象
                ViewBag.CurrentProductType = db.ProductType.Find(ProductTypeId);
            }

            // 如果请求中 不带有 排序 Order 参数, 那么默认设置为 按销量降序排序
            if( Order == null )
            {
                Order = 1;
            }
            // 排序方式 ， 1 : 销量降序， 2 : 价格升序 , 3: 价格降序
            switch ( Order )
            {
                case 1:
                    list = list.OrderByDescending(p => p.Sales);
                    break;
                case 2:
                    list = list.OrderBy(p => p.NewPrice);
                    break;
                case 3:
                    list = list.OrderByDescending(p => p.NewPrice);
                    break;
            }
            ViewBag.Order = Order;

            ViewBag.ProductCount = list.Count();          // 分页前商品总条数
           

            // 分页
            if (Page == null)
            {
                Page = 1;
            }
            // 获取该页的数据
            //  假设 Page = 4 , 每页 3 条
            list = list.Skip( Convert.ToInt32((Page-1)* PAGE_SIZE) ).Take(PAGE_SIZE);     
            ViewBag.pageSize = PAGE_SIZE;
            ViewBag.Page = Page;


            // 读取购物车数量
            if (Session["user"] != null)
            {
                User u = Session["user"] as User;
                var myCart = db.Cart.Where(c => c.UserId == u.Id);
                int? cartCount = 0;
                foreach (var item in myCart)
                {
                    cartCount += item.Number;
                }
                ViewBag.cartCount = cartCount;
            }


            return View(list.ToList());
        }


        /// <summary>
        /// 商品详情页
        /// </summary>
        /// <param name="id">商品编号</param>
        /// <returns></returns>
        public ActionResult Detail( int id)
        {
            // 获取商品的一级类别
            ViewBag.ProductTypes = db.ProductType.Where(pt => pt.Pid == null);

            // 读取购物车数量
            if( Session["user"] !=null)
            {
                User u = Session["user"] as User;
                var myCart = db.Cart.Where(c => c.UserId == u.Id);
                int? cartCount = 0;
                foreach (var item in myCart)
                {
                    cartCount += item.Number;
                }
                ViewBag.cartCount = cartCount;
            }

            // 获取商品详情
            Product p = db.Product.Find(id);

            return View(p);
        }

    }
}