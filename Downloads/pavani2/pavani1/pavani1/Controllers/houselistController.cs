using pavani1.Models;
using System.Web.Mvc;
using System.Linq;
using System.Web.Mvc;

namespace pavani1.Controllers
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



