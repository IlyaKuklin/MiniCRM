using MiniCRMCore.Areas.Offers.Models;
using System.Collections.Generic;

namespace MiniCRMCore.Areas.Clients.Models
{
	public class Client
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public string LegalEntitiesNames { get; set; }
		public string DomainNames { get; set; }
		public string Contact { get; set; }
		public string Diagnostics { get; set; }
		
		public virtual List<Offer> Offers { get; set; }

		public class Dto
		{
			public int Id { get; set; }

			/// <summary>
			/// Название
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Названия юрлиц.
			/// </summary>
			public string LegalEntitiesNames { get; set; }

			/// <summary>
			/// Названия сайтов и доменов.
			/// </summary>
			public string DomainNames { get; set; }

			/// <summary>
			/// Контактное лицо.
			/// </summary>
			public string Contact { get; set; }

			/// <summary>
			/// Диагностика клиента.
			/// </summary>
			public string Diagnostics { get; set; }

			/// <summary>
			/// Список КП клиента.
			/// </summary>
			public List<Offer.Dto> Offers { get; set; }
		}
	}
}