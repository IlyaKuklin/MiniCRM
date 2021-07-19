using MiniCRMCore.Areas.Clients.Models;

namespace MiniCRMCore.Areas.Offers.Models
{
	public class Offer
	{
		public int Id { get; set; }
		public int Number { get; set; }

		/// <summary>
		/// Тип товара/системы
		/// </summary>
		public string ProductSystemType { get; set; }

		/// <summary>
		/// Краткое описание отрасли клиента
		/// </summary>
		public string BriefIndustryDescription { get; set; }

		/// <summary>
		/// Кейс 
		/// </summary>
		public string OfferCase { get; set; }

		public virtual Client Client { get; set; }
		public int ClientId { get; set; }

		public class Dto
		{
			public int Id { get; set; }
			public int Number { get; set; }
			public string ProductSystemType { get; set; }
			public string BriefIndustryDescription { get; set; }
			public string OfferCase { get; set; }

			public Client.Dto Client { get; set; }
			public int ClientId { get; set; }
		}

		public class EditDto
		{
			public int Id { get; set; }
			public string ProductSystemType { get; set; }
			public string BriefIndustryDescription { get; set; }
			public string OfferCase { get; set; }

			public int ClientId { get; set; }
		}
	}
}