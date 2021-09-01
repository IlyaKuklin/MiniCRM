using MiniCRMCore.Areas.Auth.Models;
using MiniCRMCore.Areas.Common;

namespace MiniCRMCore.Areas.Offers.Models
{
	public class OfferFeedbackRequest : BaseEntity
	{
		public Offer Offer { get; set; }
		public int OfferId { get; set; }
		public string Text { get; set; }
		public User Author { get; set; }
		public int AuthorId { get; set; }

		public bool Answered { get; set; }
		public string AnswerText { get; set; }

		public class Dto : BaseDto
		{
			public string Text { get; set; }
			public User.Dto Author { get; set; }
			public bool Answered { get; set; }
			public string AnswerText { get; set; }
		}

		public class AddDto
		{
			public int OfferId { get; set; }
			public string Text { get; set; }
		}
	}
}