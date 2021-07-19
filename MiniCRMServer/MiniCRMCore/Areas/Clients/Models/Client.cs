using MiniCRMCore.Areas.Offers.Models;
using System.Collections.Generic;

namespace MiniCRMCore.Areas.Clients.Models
{
	public class Client
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public virtual List<Offer> Offers { get; set; }

		public class Dto
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public List<Offer.Dto> Offers { get; set; }
		}
	}
}