using MiniCRMCore.Areas.Auth.Models;
using MiniCRMCore.Areas.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniCRMCore.Areas.Offers.Models
{
	public class OfferVersion : BaseEntity
	{
		public int Number { get; set; }

		public string Data { get; set; }

		public Offer Offer { get; set; }
		public int OfferId { get; set; }

		public User Author { get; set; }
		public int AuthorId { get; set; }
	}
}