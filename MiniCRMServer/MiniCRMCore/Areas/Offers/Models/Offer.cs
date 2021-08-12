using MiniCRMCore.Areas.Clients.Models;
using MiniCRMCore.Areas.Common;
using System.Collections.Generic;

namespace MiniCRMCore.Areas.Offers.Models
{
	public class Offer : BaseEntity
	{
		public int Number { get; set; }
		public string ProductSystemType { get; set; }
		public string BriefIndustryDescription { get; set; }
		public string OfferCase { get; set; }

		public string Description { get; set; }
		//public List<FileDatum> Photos { get; set; }

		public string OfferPoint { get; set; }
		public string Recommendations { get; set; }
		//public FileDatum TechPassport { get; set; }
		//public List<FileDatum> Certificates { get; set; }

		public string OtherDocumentation { get; set; }
		public string CoveringLetter { get; set; }
		public string SimilarCases { get; set; }
		public string NewsLinks { get; set; }
		//public FileDatum Card { get; set; }

		public OfferPotential Potential { get; set; }
		public string Stage { get; set; }

		public virtual Client Client { get; set; }
		public int ClientId { get; set; }

		public class Dto
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

	public enum OfferPotential
	{
		Undefined = 0,
		NotSet,

		Cold,
		Warm,
		Hot
	}
}