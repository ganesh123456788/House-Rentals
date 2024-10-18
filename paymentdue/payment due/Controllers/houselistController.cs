using houselists.Models;
using System.Web.Mvc;

using System.Linq;
using System.Web.Mvc;
using houselists.Models;

namespace houselists.Controllers
{
    public class HousesController : Controller
    {
        private HouseContext db = new HouseContext();

        // GET: Houses
        public ActionResult Index()
        {
            var houses = db.Houselists.ToList(); // Fetch data from database
            return View(houses);
        }
    }
}



