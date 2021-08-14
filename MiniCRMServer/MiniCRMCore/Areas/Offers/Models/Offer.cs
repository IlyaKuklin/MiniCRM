﻿using MiniCRMCore.Areas.Auth.Models;
using MiniCRMCore.Areas.Clients.Models;
using MiniCRMCore.Areas.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
		public OfferPotential Potential { get; set; }
		public string Stage { get; set; }

		public virtual Client Client { get; set; }
		public int ClientId { get; set; }

		public List<OfferFileDatum> FileData { get; set; }

		public List<string> SelectedSections { get; set; }

		public List<OfferVersion> Versions { get; set; }
		public int CurrentVersion { get; set; }

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

			public OfferPotential Potential { get; set; }
			public string Stage { get; set; }

			public Client.Dto Client { get; set; }
			public int ClientId { get; set; }

			public List<OfferFileDatum.Dto> FileData { get; set; }

			[Required]
			public List<string> SelectedSections { get; set; }
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

			public OfferPotential Potential { get; set; }
			public string Stage { get; set; }
		}

		public class UpdateDto
		{
			[Required]
			public Dto EditDto { get; set; }

			[Required]
			public List<string> SelectedSections { get; set; }
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