using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace web_da544B.Models
{
	//use about page
	public class BookInfo
	{
		public int ID { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public double Price { get; set; }
		public int Year { get; set; }
		public int ISBNNo { get; set; }
		public string CoverPageImageUrl { get; set; }
		public string BookCategory { get; set; }

		[NotMapped]
		public int QuantityPerOrder { get; set; }

		[NotMapped]
		public double PricePerOrder { get; set; }
	}


	//public class BookCategory
	//{
	//	public int ID { get; set; }
	//	public string Description { get; set; }
	//}

	//Send an instancs of Order to Queue and web job 
	//will update db for order and create row in notification
	public class Order
	{
		public int ID { get; set; }

		public string CustomerEmail { get; set; }

		public List<BookInfo> BookInfos { get; set; }

		public double TotalPrice { get; set; } //if total price is more than 500kr give 10% discount

		public int Quantity { get; set; }

		public string Category { get; set; }

		public DateTime CreatedAT { get; set; }

	}

	//home page pop up with order confirmation 
	//if isRead is false and makes it true after pops up
	public class Notification
	{
		public int ID { get; set; }
		public int OrderID { get; set; }
		public bool isRead { get; set; }
	}




	public class BookContext : DbContext
	{
		public DbSet<BookInfo> BookInfos { get; set; }
		//public DbSet<BookCategory> BookCategories { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Notification> Notifications { get; set; }

	}
}