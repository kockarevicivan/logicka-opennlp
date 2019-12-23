using Logicka.Core.Entities;
using Logicka.WebAPI.Factories;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Logicka.WebAPI.Controllers
{
    public class TrainController : Controller
    {
        private LHippocampus _context;

        public TrainController()
        {
            _context = ContextFactory.GetInstance().GetContext();
        }

        [HttpPost]
        public ActionResult Submit(string statement)
        {
            if (statement == "save")
            {
                _context.SaveToFile(ConfigurationManager.AppSettings["LODBLocation"] + "saved" + DateTime.Now.Ticks + ".lodb");
                return Json("Saved.");
            }

            string result = _context.Submit(statement).ToString();

            return Json(result);
        }

        [HttpPost]
        public ActionResult Query(string query)
        {
            var result = _context.Query(query).FirstOrDefault();

            if (result != null)
                return Json(result.ToString());
            else
                return Json("Nothing found.");
        }
    }
}