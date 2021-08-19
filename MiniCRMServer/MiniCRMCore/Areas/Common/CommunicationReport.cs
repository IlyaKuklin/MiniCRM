using MiniCRMCore.Areas.Auth.Models;
using System.ComponentModel.DataAnnotations;

namespace MiniCRMCore.Areas.Common
{
	public class CommunicationReport : BaseEntity
	{
		public string Text { get; set; }

		public int AuthorId { get; set; }
		public User Author { get; set; }

		public class Dto : BaseDto
		{
			public string Text { get; set; }
			public User.Dto Author { get; set; }
			public int ClientId { get; set; }
			public int OfferId { get; set; }
		}

		public class EditDto
		{
			[Required]
			public int Id { get; set; }

			[Required]
			public string Text { get; set; }

			[Required]
			public int ClientId { get; set; }

			[Required]
			public int OfferId { get; set; }
		}
	}
}