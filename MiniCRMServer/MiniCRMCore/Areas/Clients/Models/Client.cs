using MiniCRMCore.Areas.Common;
using MiniCRMCore.Areas.Offers.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiniCRMCore.Areas.Clients.Models
{
	public class Client : BaseEntity
	{
		public string Name { get; set; }

		public string LegalEntitiesNames { get; set; }
		public string DomainNames { get; set; }
		public string Contact { get; set; }
		public string Diagnostics { get; set; }

		public virtual List<Offer> Offers { get; set; }

		public virtual List<CommunicationReport> CommonCommunicationReports { get; set; }

		public string Key { get; set; }

		public class Dto
		{
			[Required]
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

			[Required]
			/// <summary>
			/// Отчёты о коммуникации с клиентом.
			/// </summary>
			public virtual List<CommunicationReport.Dto> CommonCommunicationReports { get; set; }
		}
	}
}