﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AreaController : Controller
    {
        private Entities db = new Entities();

        // GET: Area
        public ActionResult List( int? AreaId )
        {
            return Json(
                db.Area.Where(a => a.ParentId == AreaId).Select( a => new {
                    Id = a.Id ,
                    Name = a.Name ,
                    ParentId = a.ParentId
                } ) , 
                JsonRequestBehavior.AllowGet 
            );
        }
    }
}