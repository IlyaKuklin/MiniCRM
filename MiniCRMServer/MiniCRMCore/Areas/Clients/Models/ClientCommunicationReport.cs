using MiniCRMCore.Areas.Auth.Models;
using MiniCRMCore.Areas.Common;

namespace MiniCRMCore.Areas.Clients.Models
{
	public class ClientCommunicationReport : BaseEntity
	{
		public string Text { get; set; }

		public int AuthorId { get; set; }
		public User Author { get; set; }

		public int ClientId { get; set; }
		public Client Client { get; set; }

		public class Dto : BaseDto
		{
			public string Text { get; set; }
			public User.Dto Author { get; set; }
			public Client.Dto Client { get; set; }
		}
	}
}