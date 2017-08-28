using System.Linq;
using System.Web.Mvc;
using web_da544B.Models;

namespace web_da544B.Controllers
{
	public class OrderController : Controller
	{
		// GET: Order
		public ActionResult Index()
		{
			var db = new BookContext();
			//current authenticated user and filter order by customer ID

			var x = db.Orders.ToList();
			return View(x);
		}


	
	}
}