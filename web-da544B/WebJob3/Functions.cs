using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using web_da544B.Models;

namespace WebJob3
{
	public class Functions
	{
		// This function will get triggered/executed when a new message is written 
		// on an Azure Queue called queue.
		public static void ProcessQueueMessage([QueueTrigger("greet")] string notify, TextWriter log)
		{
			Database.SetInitializer<BookContext>(null);
			var notification = JsonConvert.DeserializeObject<Notification>(notify);
			BookContext bookContext = new BookContext();

			//store notification that should be grabed from client and pops up notification message

			bookContext.Notifications.Add(notification);
			bookContext.SaveChanges();

			log.WriteLine(notify);
		}
	}
}
