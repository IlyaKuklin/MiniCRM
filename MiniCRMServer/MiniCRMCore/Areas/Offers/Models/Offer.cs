using MiniCRMCore.Areas.Clients.Models;
using MiniCRMCore.Areas.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiniCRMCore.Areas.Offers.Models
{

	public class Offer : BaseEntity
	{
		public int Number { get; set; }
		public string ProductSystemType { get; set; }
		public string BriefIndustryDescription { get; set; }
		public string OfferCase { get; set; }
		public string Description { get; set; }
		public string OfferPoint { get; set; }
		public string Recommendations { get; set; }
		public string OtherDocumentation { get; set; }
		public string CoveringLetter { get; set; }
		public string SimilarCases { get; set; }
		public string NewsLinks { get; set; }
		public string Potential { get; set; }
		public string Stage { get; set; }

		public virtual Client Client { get; set; }
		public int ClientId { get; set; }

		public List<OfferFileDatum> FileData { get; set; }

		public List<string> SelectedSections { get; set; }

		public int CurrentVersionNumber { get; set; }
		public int ClientVersionNumber { get; set; }

		public List<OfferVersion> Versions { get; set; }

		public Guid ClientLink { get; set; }

		public List<OfferNewsbreak> Newsbreaks { get; set; }
		public List<OfferFeedbackRequest> FeedbackRequests { get; set; }

		public List<OfferRule> Rules { get; set; }

		public virtual List<CommunicationReport> CommonCommunicationReports { get; set; }

		public class ClientViewDto
		{
			[Required]
			public int Number { get; set; }
			[Required]
			public DateTime Changed { get; set; }
			[Required]
			public string ManagerEmail { get; set; }
			[Required]
			public List<SectionDto> Sections { get; set; }
			[Required]
			public Client.Dto Client { get; set; }
			[Required]
			public List<OfferFeedbackRequest.Dto> FeedbackRequests { get; set; }
		}

		public class Dto : BaseDto
		{
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

			public string Description { get; set; }

			/// <summary>
			/// Суть предложения
			/// </summary>
			public string OfferPoint { get; set; }

			/// <summary>
			/// Рекомендации клиенту
			/// </summary>
			public string Recommendations { get; set; }

			/// <summary>
			/// Прочая документация
			/// </summary>
			public string OtherDocumentation { get; set; }

			/// <summary>
			/// Сопроводительное письмо
			/// </summary>
			public string CoveringLetter { get; set; }

			/// <summary>
			/// Аналогичный кейсы
			/// </summary>
			public string SimilarCases { get; set; }

			/// <summary>
			/// Новости
			/// </summary>
			public string NewsLinks { get; set; }

			/// <summary>
			/// Тип КП (Холодный / Теплый / Горячий).
			/// </summary>
			public string Potential { get; set; }

			public string Stage { get; set; }

			public Client.Dto Client { get; set; }
			public int ClientId { get; set; }

			public List<OfferFileDatum.Dto> FileData { get; set; }

			[Required]
			public List<string> SelectedSections { get; set; }

			/// <summary>
			/// Ссылка для клиента.
			/// </summary>
			public Guid ClientLink { get; set; }

			/// <summary>
			/// Инфоповоды.
			/// </summary>
			public List<OfferNewsbreak.Dto> Newsbreaks { get; set; }

			/// <summary>
			/// Заявки на обратную связь с клиентом.
			/// </summary>
			public List<OfferFeedbackRequest.Dto> FeedbackRequests { get; set; }

			[Required]
			/// <summary>
			/// Правила работы с клиентом.
			/// </summary>
			public List<OfferRule.Dto> Rules { get; set; }

			[Required]
			public virtual List<CommunicationReport.Dto> CommonCommunicationReports { get; set; }
		}

		public class EditDto
		{
			public int Id { get; set; }
			public string ProductSystemType { get; set; }
			public string BriefIndustryDescription { get; set; }
			public string OfferCase { get; set; }
			public string Description { get; set; }

			/// <summary>
			/// Суть предложения
			/// </summary>
			public string OfferPoint { get; set; }

			/// <summary>
			/// Рекомендации клиенту
			/// </summary>
			public string Recommendations { get; set; }

			/// <summary>
			/// Прочая документация
			/// </summary>
			public string OtherDocumentation { get; set; }

			/// <summary>
			/// Сопроводительное письмо
			/// </summary>
			public string CoveringLetter { get; set; }

			/// <summary>
			/// Аналогичный кейсы
			/// </summary>
			public string SimilarCases { get; set; }

			public int ClientId { get; set; }

			/// <summary>
			/// Новости
			/// </summary>
			public string NewsLinks { get; set; }

			public string Potential { get; set; }
			public string Stage { get; set; }
		}
	}

	public class SectionDto
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Data { get; set; }

		[Required]
		public List<string> ImagePaths { get; set; }

		[Required]
		public string Type { get; set; }
	}
}