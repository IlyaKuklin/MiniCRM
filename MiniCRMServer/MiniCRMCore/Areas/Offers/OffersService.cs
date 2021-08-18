using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MiniCRMCore.Areas.Common;
using MiniCRMCore.Areas.Offers.Models;
using MiniCRMCore.Utilities.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCRMCore.Areas.Offers
{
	public class OffersService
	{
		private readonly ApplicationContext _context;
		private readonly IMapper _mapper;

		public OffersService(ApplicationContext context, IMapper mapper)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		#region Offers

		public async Task<Offer.Dto> GetAsync(int id)
		{
			var offer = await _context.Offers
				.Include(x => x.Client)
				.Include(x => x.FileData)
					.ThenInclude(x => x.FileDatum)
				.Include(x => x.Newsbreaks)
					.ThenInclude(x => x.Author)
				.Include(x => x.FeedbackRequests)
					.ThenInclude(x => x.Author)
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);
			if (offer == null)
				throw new ApiException($"Не найден клиент с ID {id}");

			var dto = _mapper.Map<Offer.Dto>(offer);
			return dto;
		}

		public async Task<List<Offer.Dto>> GetListAsync()
		{
			var offers = await _context.Offers
				.Include(x => x.Client)
				.AsNoTracking()
				.ToListAsync();

			var dto = _mapper.Map<List<Offer.Dto>>(offers);
			return dto;
		}

		public async Task<Offer.Dto> EditAsync(Offer.Dto dto, int currentUserId)
		{
			Offer offer;
			if (dto.Id > 0)
			{
				offer = await _context.Offers
					.Include(x => x.Client)
					.Include(x => x.FileData)
						.ThenInclude(x => x.FileDatum)
					.Include(x => x.Versions)
					.FirstOrDefaultAsync(x => x.Id == dto.Id);
				if (offer == null)
					throw new ApiException($"Не найдено КП с ID {dto.Id}");
			}
			else
			{
				offer = new Offer { Number = 777, Versions = new List<OfferVersion>() };
				var lastOffer = await _context.Offers.OrderBy(x => x.Id).AsNoTracking().LastOrDefaultAsync();
				if (lastOffer != null)
					offer.Number = lastOffer.Number + 1;
				await _context.Offers.AddAsync(offer);
			}

			_mapper.Map(dto, offer);

			var jsonVersion = JsonConvert.SerializeObject(offer, new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			});
			offer.CurrentVersion++;
			var dbVersion = new OfferVersion
			{
				Data = jsonVersion,
				Number = offer.CurrentVersion,
				AuthorId = currentUserId
			};
			offer.Versions.Add(dbVersion);
			//offer.Versions.Add()

			//await _context.OfferVersions.AddAsync(new OfferVersion
			//{
			//	Data = jsonVersion,
			//	OfferId = offer.Id,
			//	Number = offer.CurrentVersion,
			//	AuthorId = currentUserId
			//});

			await _context.SaveChangesAsync();

			var returnDto = _mapper.Map<Offer.Dto>(offer);
			return returnDto;
		}

		public async Task DeleteAsync(int id)
		{
			var offer = await _context.Offers.FirstOrDefaultAsync(x => x.Id == id);
			if (offer == null)
				throw new ApiException($"Не найдено КП с ID {id}");

			_context.Offers.Remove(offer);
			await _context.SaveChangesAsync();
		}

		#endregion

		#region Files

		public async Task<List<OfferFileDatum.Dto>> UploadFileAsync(List<IFormFile> files, int id, OfferFileType type, bool replace)
		{
			var offer = await _context.Offers
				.Include(x => x.FileData)
					.ThenInclude(x => x.FileDatum)
				.FirstOrDefaultAsync(x => x.Id == id);
			if (offer == null)
				throw new ApiException($"Не найдено КП с ID {id}");

			var offerFilesDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", id.ToString(), type.ToString());
			if (!Directory.Exists(offerFilesDirectoryPath))
				Directory.CreateDirectory(offerFilesDirectoryPath);

			if (replace)
			{
				var currentFiles = Directory.GetFiles(offerFilesDirectoryPath);
				foreach (var currentFile in currentFiles)
					File.Delete(currentFile);

				var filesToDelete = offer.FileData.Where(x => x.Type == type);
				_context.OfferFileData.RemoveRange(filesToDelete);
				_context.FileData.RemoveRange(filesToDelete.Select(x => x.FileDatum));
			}

			var newOfferFiles = new List<OfferFileDatum>();
			foreach (var file in files)
			{
				var fullFilePath = Path.Combine(offerFilesDirectoryPath, file.FileName);
				var relativeFilePath = Path.Combine("files", id.ToString(), type.ToString(), file.FileName);
				using (var stream = File.Create(fullFilePath))
				{
					await file.CopyToAsync(stream);
					var newOfferFile = new OfferFileDatum
					{
						FileDatum = new FileDatum
						{
							Name = file.FileName,
							Path = relativeFilePath
						},
						Type = type
					};
					offer.FileData.Add(newOfferFile);
					newOfferFiles.Add(newOfferFile);
				}
			}

			var sectionKey = string.Empty;
			switch (type)
			{

				case OfferFileType.Photo: sectionKey = "description"; break;
				case OfferFileType.TechPassport: sectionKey = "techPassport"; break;
				case OfferFileType.Certificate: sectionKey = "certificate"; break;
				case OfferFileType.Card: sectionKey = "card"; break;
			}
			if (!offer.SelectedSections.Contains(sectionKey))
				offer.SelectedSections.Add(sectionKey);

			await _context.SaveChangesAsync();

			var dto = _mapper.Map<List<OfferFileDatum.Dto>>(newOfferFiles);

			return dto;
		}

		public async Task DeleteFileAsync(int offerFileId)
		{
			var offerFile = await _context.OfferFileData
				.Include(x => x.FileDatum)
				.FirstOrDefaultAsync(x => x.Id == offerFileId);
			if (offerFile == null)
				throw new ApiException($"Не найден файл с ID {offerFileId}");

			_context.OfferFileData.Remove(offerFile);
			_context.FileData.Remove(offerFile.FileDatum);

			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", offerFile.FileDatum.Path);
			File.Delete(path);

			await _context.SaveChangesAsync();
		}

		#endregion

		public async Task<OfferNewsbreak.Dto> AddOfferNewsbreakAsync(OfferNewsbreak.AddDto addDto, int currentUserId)
		{
			var offer = await _context.Offers
				.Include(x => x.Newsbreaks)
				.FirstOrDefaultAsync(x => x.Id == addDto.OfferId);
			if (offer == null)
				throw new ApiException($"Не найдено КП с ID {addDto.OfferId}");

			var user = await _context.Users.FirstAsync(x => x.Id == currentUserId);

			var newsbreak = new OfferNewsbreak
			{
				Text = addDto.Text,
				AuthorId = currentUserId,
				Author = user
			};

			offer.Newsbreaks.Add(newsbreak);
			await _context.SaveChangesAsync();

			var dto = _mapper.Map<OfferNewsbreak.Dto>(newsbreak);
			return dto;
		}

		public async Task<OfferFeedbackRequest.Dto> AddOfferFeedbackRequestAsync(OfferFeedbackRequest.AddDto addDto, int currentUserId)
		{
			var offer = await _context.Offers
				.Include(x => x.FeedbackRequests)
				.FirstOrDefaultAsync(x => x.Id == addDto.OfferId);
			if (offer == null)
				throw new ApiException($"Не найдено КП с ID {addDto.OfferId}");

			var user = await _context.Users.FirstAsync(x => x.Id == currentUserId);

			var request = new OfferFeedbackRequest
			{
				Text = addDto.Text,
				AuthorId = currentUserId,
				Author = user
			};

			offer.FeedbackRequests.Add(request);
			await _context.SaveChangesAsync();

			var dto = _mapper.Map<OfferFeedbackRequest.Dto>(request);
			return dto;
		}

		#region ClientView

		public async Task<Offer.ClientViewDto> GetOfferForClientAsync(Guid link, string clientKey)
		{
			var offer = await _context.Offers
				.Include(x => x.Client)
				.Include(x => x.FileData)
					.ThenInclude(x => x.FileDatum)
				.Include(x => x.Versions)
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.ClientLink == link);

			if (offer == null)
				throw new ApiException($"Не найдено КП по ссылке {link}");

			//if (offer.Client.Key != clientKey)
			//	throw new ApiException("", 403);

			var lstV = offer.Versions.Last();

			var versionToDisplay = JsonConvert.DeserializeObject<Offer>(lstV.Data);

			var type = typeof(Offer);
			var props = typeof(Offer).GetProperties();

			var dto = new Offer.ClientViewDto
			{
				Sections = new List<SectionDto>()
			};


			foreach (var sectionName in versionToDisplay.SelectedSections)
			{
				var section = new SectionDto
				{
					Name = GetReadablePropertyName(sectionName),
					Type = "text"
				};

				var textProperty = type.GetProperty(sectionName.FirstCharToUpper());
				if (textProperty != null)
				{
					var value = textProperty.GetValue(versionToDisplay);
					if (value != null)
					{
						section.Data = value.ToString();
						dto.Sections.Add(section);
					}
				}

				else
				{
					if (sectionName == "techPassport")
					{
						section.Type = "img";
						section.ImagePaths = versionToDisplay.FileData.Where(x => x.Type == OfferFileType.TechPassport).Select(x => x.FileDatum.Path).ToList();
						dto.Sections.Add(section);
					}
				}
				
			}

			return dto;
		}

		private static string GetReadablePropertyName(string name)
		{
			switch (name)
			{
				case "productSystemType": return "Тип товара/системы";

				case "briefIndustryDescription": return "Краткое описание отрасли";
				case "offerCase": return "Кейс";
				case "offerPoint": return "Суть предложения";
				case "recommendations": return "Рекомендации";
				case "techPassport": return "Технический паспорт";
				case "coveringLetter": return "Сопроводительное письмо";
				case "similarCases": return "Аналогичные кейсы";
				
			}

			return "NotImplementedException";
			//throw new NotImplementedException();
		}

		#endregion
	}

	public static class StringExtentions
	{
		public static string FirstCharToUpper(this string input)
		{
			return input switch
			{
				null => throw new ArgumentNullException(nameof(input)),
				"" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
				_ => input.First().ToString().ToUpper() + input.Substring(1)
			};
		}
	}
}