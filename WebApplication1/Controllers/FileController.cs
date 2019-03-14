using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files)
        {
            
            int i = 0;
            foreach (var file in files)
            {
                file.SaveAs("d:/"+ i++ +".jpg");
            }
            
            return Content( files.ToList().Count + "OK");
        }
    }
}