using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using web_da544B.Models;

namespace web_da544B.Controllers
{
	public class BookInfoesController : AccountController
	{
		readonly BookContext _bookContext = new BookContext();

		BookContext db = new BookContext();
		List<BookInfo> books;
		// GET: BookInfoes
		public ActionResult Index()
		{

			//var bks = db.BookInfos.ToList();
			//if (!bks.Any())
			//{
			books = new List<BookInfo>()
				{
					new BookInfo()
					{
						Title = "Book A",
						BookCategory = "Scientific",
						Year = 2009,
						Author = "MS ds R",
						Price = 2200,
						ISBNNo = 33663

					},
					new BookInfo()
					{
						Title = "Book B",
						BookCategory = "Mathematics",
						Year = 2003,
						Author = "MS dun",
						Price = 330,
						ISBNNo = 313
					},
					new BookInfo()
					{
						Title = "Book C",
						BookCategory = "Scientific",
						Year = 2015,
						Author = "Li d",
						Price = 250,
						ISBNNo = 323

					}
			};

			books.ForEach(x => db.BookInfos.Add(x));
			db.SaveChanges();

			return View(books);
		}


		public ActionResult PrepareOrder(string[] itemIds)
		{
			//splitting ids and form it into list
			if (itemIds == null || itemIds.FirstOrDefault() == string.Empty || !itemIds.Any())
			{
				return View("Index");
			}
			var bookIds = new List<int>();
			if (itemIds.ToList().Count > 0)
			{
				bookIds = itemIds[0].Split(',').ToList().Select(int.Parse).ToList();
			}

			var distinctList = bookIds.Distinct().ToList();
			//var orderedItems = new Dictionary<BookInfo, int>();

			List<BookInfo> bookInfos = new List<BookInfo>();
			foreach (var id in distinctList)
			{
				var book = _bookContext.BookInfos.FirstOrDefault(x => x.ID == id);

				if (book == null)
					continue;

				var count = bookIds.Count(i => i.Equals(id));
				book.QuantityPerOrder = count;
				book.PricePerOrder = book.Price * book.QuantityPerOrder;
				bookInfos.Add(book);
			}
			double amountToPay = 0;

			amountToPay = bookInfos.Select(x => x.Price).ToList().OrderByDescending(x => x).Sum();

			var o = new Order()
			{
				BookInfos = bookInfos,
				TotalPrice = amountToPay,
				Quantity = bookIds.Count,
				CustomerEmail = User.Identity.Name
			};

			_bookContext.Orders.Add(o);
			_bookContext.SaveChanges();

			ViewBag.amount = amountToPay;
			ViewBag.CategoryNames = bookInfos.Select(x => x.BookCategory).ToList();

			//send queue
			CloudStorageAccount storageAccount =
						CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());
			CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

			//Act
			CloudQueue queue = queueClient.GetQueueReference("greet");
			queue.CreateIfNotExists();

			var n = new Notification()
			{
				OrderID = o.ID,
				isRead = false
			};

			var queueMessage = new CloudQueueMessage(JsonConvert.SerializeObject(n));
			queue.AddMessage(queueMessage, null);

			//save notification in web job
			//_bookContext.Notifications.Add(n);
			//_bookContext.SaveChanges();

			ViewBag.OrderId = o.ID;
			return View(bookInfos);
		}

		public ActionResult SearchView(int searchvalue)
		{
			var v = _bookContext.BookInfos.Where(x => x.ISBNNo == searchvalue).OrderBy(r => r.BookCategory).ToList();
			return View(v);
		}


		//need to get exact notification some how
		public ActionResult GetVerification(int OrderId)
		{
			var n = _bookContext.Notifications.FirstOrDefault(x => x.OrderID == OrderId);

			if (n != null && n.isRead == false)
			{
				//return pop up in the view
				ViewBag.notify = "Order Id " + OrderId + " has been Paid!";

				n.isRead = true;
				_bookContext.SaveChanges();
			}

			return View();
		}
	}
}