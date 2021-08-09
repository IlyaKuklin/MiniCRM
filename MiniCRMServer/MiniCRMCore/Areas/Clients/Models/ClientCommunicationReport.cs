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
	}
}