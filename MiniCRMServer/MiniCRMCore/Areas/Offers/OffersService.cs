using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MiniCRMCore.Areas.Auth.Models;
using MiniCRMCore.Areas.Common;
using MiniCRMCore.Areas.Common.Interfaces;
using MiniCRMCore.Areas.Email;
using MiniCRMCore.Areas.Offers.Models;
using MiniCRMCore.Utilities.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace MiniCRMCore.Areas.Offers
{
	public class OffersService
	{
		private readonly ApplicationContext _context;
		private readonly IMapper _mapper;
		private readonly EmailSenderService _emailSenderService;
		private readonly IConfiguration _configuration;
		private readonly IWorkingDaysResolver _workingDaysResolver;

		public OffersService(
			ApplicationContext context,
			IMapper mapper,
			EmailSenderService emailSenderService,
			IConfiguration configuration,
			IWorkingDaysResolver workingDaysResolver
			)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_emailSenderService = emailSenderService ?? throw new ArgumentNullException(nameof(emailSenderService));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_workingDaysResolver = workingDaysResolver ?? throw new ArgumentNullException(nameof(workingDaysResolver));
		}

		public string BasePath
		{
			get
			{
				var section = _configuration.GetSection("BasePath");
				return section.Value;
			}
		}

		#region Offers

		public async Task<Offer.Dto> GetAsync(int id, int currentUserId)
        {
            var offer = await _context.Offers
                .Include(x => x.Client)
                .Include(x => x.FileData)
                    .ThenInclude(x => x.FileDatum)
                .Include(x => x.Newsbreaks)
                    .ThenInclude(x => x.Author)
                .Include(x => x.FeedbackRequests)
                    .ThenInclude(x => x.Author)
                .Include(x => x.CommonCommunicationReports)
                    .ThenInclude(x => x.Author)
                .Include(x => x.Rules)
                .Include(x => x.Manager)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (offer == null)
                throw new ApiException($"Не найден клиент с ID {id}");

            await CheckUserAccessAsync(currentUserId, offer);

            var dto = _mapper.Map<Offer.Dto>(offer);
            return dto;
        }

        public async Task<List<Offer.Dto>> GetListAsync(int currentUserId)
		{
			var currentManager = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == currentUserId);
			if (currentManager == null)
				throw new ApiException("Не найден пользователь");

			var offers = _context.Offers
				.Include(x => x.Client)
				.AsNoTracking();

			if (!currentManager.AllowedToViewAllOffers)
				offers = offers.Where(x => x.ManagerId == currentUserId);

			var result = await offers.ToListAsync();

			var dto = _mapper.Map<List<Offer.Dto>>(result);
			return dto;
		}

		public async Task<List<Offer.ShortDto>> GetListAsync(string filter, int currentUserId)
		{
			var currentManager = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == currentUserId);
			if (currentManager == null)
				throw new ApiException("Не найден пользователь");

			IQueryable<Offer> offers;
			if (string.IsNullOrEmpty(filter))
			{
				offers = _context.Offers
					.Include(x => x.Client)
					.Include(x => x.Versions)
					.AsNoTracking();
			}
			else
			{
				Expression<Func<Offer, bool>> predicate = x =>
					x.Client.Name.Contains(filter) ||
					x.Number.ToString().Contains(filter);

				offers = _context.Offers
					.Include(x => x.Client)
					.Include(x => x.Versions)
					.Where(predicate)
					.AsNoTracking();
			}

			if (!currentManager.AllowedToViewAllOffers)
				offers = offers.Where(x => x.ManagerId == currentUserId);

			var dto = _mapper.ProjectTo<Offer.ShortDto>(offers)
				.ToList();

            foreach (var item in dto)
            {
				if (item.Versions.Count == 0) continue;
				if (item.ClientVersionNumber == 0) item.Status = "Не отправлялось";

                else
                {
					var firstClientVersion = item.Versions.OrderBy(x => x.Number).First(x => x.SentToClient);
					var lastClientVersion = item.Versions.OrderBy(x => x.Number).Last(x => x.SentToClient);

					if (firstClientVersion.Number == lastClientVersion.Number)
						item.Status = firstClientVersion.VisitedByClient ? "Просмотрено" : "Не просмотрено";
					else
                    {
						if (firstClientVersion.VisitedByClient && lastClientVersion.VisitedByClient)
							item.Status = "Просмотрено / Просмотрено после изменений";
						else if (firstClientVersion.VisitedByClient && !lastClientVersion.VisitedByClient)
							item.Status = "Просмотрено / Не просмотрено после изменений";
						else if (!firstClientVersion.VisitedByClient && !lastClientVersion.VisitedByClient)
							item.Status = "Не просмотрено / Не просмотрено после изменений";
						else if (!firstClientVersion.VisitedByClient && lastClientVersion.VisitedByClient)
							item.Status = "Не просмотрено / Просмотрено после изменений";

					}
				}
            }

			//var dto = _mapper.Map<List<Offer.Dto>>(offers);
			return dto;
		}

		public async Task<Offer.Dto> EditAsync(Offer.Dto dto, bool sentToClient, int currentUserId)
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

				await CheckUserAccessAsync(currentUserId, offer);
			}
			else
			{
				offer = new Offer { Number = 777, Versions = new List<OfferVersion>(), ClientLink = Guid.NewGuid() };
				var lastOffer = await _context.Offers.OrderBy(x => x.Id).AsNoTracking().LastOrDefaultAsync();
				if (lastOffer != null)
					offer.Number = lastOffer.Number + 1;
				offer.ManagerId = currentUserId;
				await _context.Offers.AddAsync(offer);
			}

			_mapper.Map(dto, offer);

			var versions = new List<OfferVersion>();
			versions.AddRange(offer.Versions);
			offer.Versions.Clear();

			var jsonVersion = JsonConvert.SerializeObject(offer, new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			});
			offer.Versions = versions;

			offer.CurrentVersionNumber++;
			var dbVersion = new OfferVersion
			{
				Data = jsonVersion,
				Number = offer.CurrentVersionNumber,
				AuthorId = currentUserId,
				SentToClient = sentToClient
			};
			offer.Versions.Add(dbVersion);

			await _context.SaveChangesAsync();

			var returnDto = _mapper.Map<Offer.Dto>(offer);
			return returnDto;
		}

		public async Task DeleteAsync(int id, int currentUserId)
		{
			var offer = await _context.Offers.FirstOrDefaultAsync(x => x.Id == id);
			if (offer == null)
				throw new ApiException($"Не найдено КП с ID {id}");

			await CheckUserAccessAsync(currentUserId, offer);

			_context.Offers.Remove(offer);
			await _context.SaveChangesAsync();
		}

		#endregion Offers

		#region Files

		public async Task<List<OfferFileDatum.Dto>> UploadFileAsync(List<IFormFile> files, int id, OfferFileType type, bool replace, int currentUserId)
		{
			var offer = await _context.Offers
				.Include(x => x.FileData)
					.ThenInclude(x => x.FileDatum)
				.FirstOrDefaultAsync(x => x.Id == id);
			if (offer == null)
				throw new ApiException($"Не найдено КП с ID {id}");

			await CheckUserAccessAsync(currentUserId, offer);

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

		public async Task DeleteFileAsync(int offerFileId, int currentUserId)
		{
			var offerFile = await _context.OfferFileData
				.Include(x => x.FileDatum)
				.Include(x => x.Offer)
				.FirstOrDefaultAsync(x => x.Id == offerFileId);
			if (offerFile == null)
				throw new ApiException($"Не найден файл с ID {offerFileId}");

			await CheckUserAccessAsync(currentUserId, offerFile.Offer);

			_context.OfferFileData.Remove(offerFile);
			_context.FileData.Remove(offerFile.FileDatum);

			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", offerFile.FileDatum.Path);
			File.Delete(path);

			await _context.SaveChangesAsync();
		}

		#endregion Files

		#region Newsbreaks

		public async Task<OfferNewsbreak.Dto> AddOfferNewsbreakAsync(OfferNewsbreak.AddDto addDto, int currentUserId)
		{
			var offer = await _context.Offers
				.Include(x => x.Newsbreaks)
				.FirstOrDefaultAsync(x => x.Id == addDto.OfferId);
			if (offer == null)
				throw new ApiException($"Не найдено КП с ID {addDto.OfferId}");

			var user = await _context.Users.FirstAsync(x => x.Id == currentUserId);

			await CheckUserAccess(user, offer);

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

		#endregion Newsbreaks

		#region FeedbackRequests

		public async Task<OfferFeedbackRequest.Dto> AddOfferFeedbackRequestAsync(OfferFeedbackRequest.AddDto addDto, int currentUserId)
		{
			var offer = await _context.Offers
				.Include(x => x.FeedbackRequests)
				.FirstOrDefaultAsync(x => x.Id == addDto.OfferId);
			if (offer == null)
				throw new ApiException($"Не найдено КП с ID {addDto.OfferId}");

			var user = await _context.Users.FirstAsync(x => x.Id == currentUserId);

			await CheckUserAccess(user, offer);

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

		#endregion FeedbackRequests

		#region Rules

		public async Task<OfferRule.Dto> EditOfferRuleAsync(OfferRule.Dto dto, int currentUserId)
		{
			var offer = await _context.Offers
				.Include(x => x.Rules)
				.FirstOrDefaultAsync(x => x.Id == dto.OfferId);
			if (offer == null)
				throw new ApiException($"Не найдено КП с ID {dto.OfferId}");

			await CheckUserAccessAsync(currentUserId, offer);

			OfferRule rule;
			if (dto.Id > 0)
			{
				rule = offer.Rules.FirstOrDefault(x => x.Id == dto.Id);
			}
			else
			{
				rule = new OfferRule();
				offer.Rules.Add(rule);
			}

			_mapper.Map(dto, rule);
			await _context.SaveChangesAsync();

			var returnDto = _mapper.Map<OfferRule.Dto>(rule);
			return returnDto;
		}

		public async Task ChangeOfferRuleStateAsync(int ruleId, int currentUserId)
		{
			var rule = await _context.OfferRules
				.Include(x => x.Offer)
				.FirstOrDefaultAsync(x => x.Id == ruleId);
			if (rule == null)
				throw new ApiException($"На нейдено правило с ID {ruleId}");

			await CheckUserAccessAsync(currentUserId, rule.Offer);

			rule.Completed = !rule.Completed;
			await _context.SaveChangesAsync();
		}

		public async Task DeleteOfferRuleAsync(int ruleId, int currentUserId)
		{
			var rule = await _context.OfferRules
				.Include(x => x.Offer)
				.FirstOrDefaultAsync(x => x.Id == ruleId);
			if (rule == null)
				throw new ApiException($"На нейдено правило с ID {ruleId}");

			await CheckUserAccessAsync(currentUserId, rule.Offer);

			_context.OfferRules.Remove(rule);
			await _context.SaveChangesAsync();
		}

		public async Task CompleteOfferRuleAsync(OfferRule.CompleteDto dto, int currentUserId)
		{
			var rule = await _context.OfferRules
				.Include(x => x.Offer)
				.FirstOrDefaultAsync(x => x.Id == dto.Id);
			if (rule == null)
				throw new ApiException($"На нейдено правило с ID {dto.Id}");

			await CheckUserAccessAsync(currentUserId, rule.Offer);

			rule.Report = dto.Report;
			rule.Completed = true;
			await _context.SaveChangesAsync();
		}

		#endregion Rules

		#region Client

		public async Task<string> SendOfferToClientAsync(int id, int currentUserId)
		{
			var offer = await _context.Offers
				.Include(x => x.Client)
				.Include(x => x.Versions)
				.FirstOrDefaultAsync(x => x.Id == id);

			await CheckUserAccessAsync(currentUserId, offer);

			offer.ClientVersionNumber = offer.CurrentVersionNumber;

			var paramsString = $"{offer.ClientLink}/{offer.Client.Key}";

			await _context.SaveChangesAsync();

			var link = $"{this.BasePath}offers/{paramsString}";

			_emailSenderService.NotifyClient(offer.Client.Name, offer.Email, "Коммерческое предложение от АВТОМАТ СЕРВИС", paramsString);

			var nextWorkDate = await _workingDaysResolver.GetNextWorkDateAsync();
			var runtime = nextWorkDate  - DateTime.Now; 
			BackgroundJob.Schedule<OffersService>(x => 
				x.NotifyManagerIfClientDidNotOpenOffer(offer.Id), 
				runtime
				//TimeSpan.FromMinutes(5)
			);

			return link;
		}

		public async Task<Offer.ClientViewDto> GetOfferForClientAsync(Guid link, string clientKey, int currentUserId)
		{
			var offer = await _context.Offers
				.Include(x => x.Client)
				.Include(x => x.FileData)
					.ThenInclude(x => x.FileDatum)
				.Include(x => x.Versions)
					.ThenInclude(x => x.Author)
				.Include(x => x.FeedbackRequests)
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.ClientLink == link);

			if (offer == null)
				throw new ApiException($"Не найдено КП по ссылке {link}");

			if (offer.Client.Key != clientKey)
				throw new ApiException("", 403);

			var clientVersion = offer.Versions.FirstOrDefault(x => x.Number == offer.ClientVersionNumber);
			var versionToDisplay = JsonConvert.DeserializeObject<Offer>(clientVersion.Data);

			if (!clientVersion.VisitedByClient && currentUserId == 0)
			{
				var version = await _context.OfferVersions
					.Include(x => x.Author)
					.FirstAsync(x => x.Id == clientVersion.Id);
				_emailSenderService.NotifyManager(version.Author.Name, version.Author.Email, "Клиент перешёл по ссылке из письма", offer.Number, DateTime.Now);
				version.VisitedByClient = true;
				await _context.SaveChangesAsync();
			}

			var type = typeof(Offer);
			var props = typeof(Offer).GetProperties();

			var dto = _mapper.Map<Offer.ClientViewDto>(versionToDisplay);
			dto.ManagerEmail = clientVersion.Author.Email;
			dto.Sections = new List<SectionDto>();
			dto.FeedbackRequests = _mapper.Map<List<OfferFeedbackRequest.Dto>>(offer.FeedbackRequests.Where(x => !x.Answered));

			foreach (var sectionName in versionToDisplay.SelectedSections)
			{
				var section = new SectionDto
				{
					Name = GetReadablePropertyName(sectionName),
					Type = "text"
				};

				var textProperty = type.GetProperty(sectionName.FirstCharToUpper());

				if (sectionName == "description")
				{
					section.Type = "description";
					section.ImagePaths = versionToDisplay.FileData.Where(x => x.Type == OfferFileType.Photo).Select(x => x.FileDatum.Path).ToList();
					section.Data = textProperty == null ? "" : textProperty.GetValue(versionToDisplay)?.ToString();
					dto.Sections.Add(section);
					continue;
				}

				if (textProperty != null)
				{
					var value = textProperty.GetValue(versionToDisplay);
					if (value != null)
					{
						section.Data = value.ToString();
						dto.Sections.Add(section);
					}
					if (textProperty.Name == "OfferPoint")
						section.Type = "offerPoint";
				}
				else
				{
					if (sectionName == "techPassport")
					{
						section.Type = "img";
						section.ImagePaths = versionToDisplay.FileData.Where(x => x.Type == OfferFileType.TechPassport).Select(x => x.FileDatum.Path).ToList();
						dto.Sections.Add(section);
					}
					if (sectionName == "certificate")
					{
						section.Type = "img";
						section.ImagePaths = versionToDisplay.FileData.Where(x => x.Type == OfferFileType.Certificate).Select(x => x.FileDatum.Path).ToList();
						dto.Sections.Add(section);
					}
				}
			}

			return dto;
		}

		public async Task AnswerOnFeedbackRequestAsync(OfferFeedbackRequest.AnswerDto answerDto)
		{
			var request = await _context.OfferFeedbackRequests
				.Include(x => x.Offer)
					.ThenInclude(x => x.Versions)
						.ThenInclude(x => x.Author)
				.FirstOrDefaultAsync(x => x.Id == answerDto.Id);
			request.AnswerText = answerDto.AnswerText;
			request.Answered = true;
			await _context.SaveChangesAsync();

			var version = request.Offer.Versions.First(x => x.Number == request.Offer.ClientVersionNumber);
			_emailSenderService.SendEmail(version.Author.Name, version.Author.Email, "Дан ответ на обратную связь", $"По КП {request.Offer.Number} клиент дал обратную связь");
		}

		public async Task NotifyManagerIfClientDidNotOpenOffer(int offerId)
        {
			var offer = await _context.Offers
				.Include(x => x.Versions)
					.ThenInclude(x => x.Author)
				.FirstAsync(x => x.Id == offerId);

			var clientVersion = offer.Versions.First(x => x.Number == offer.ClientVersionNumber);

			if (!clientVersion.VisitedByClient)
            {
				_emailSenderService.NotifyManagerIfClientIgnoredOffer(clientVersion.Author.Name, clientVersion.Author.Email, "Клиент не ознакомился с предложением", offerId);
			}
        }

		private static string GetReadablePropertyName(string name)
		{
			switch (name)
			{
				case "productSystemType": return "Тип товара/системы";

				case "description": return "Фотографии и описание товара";
				case "briefIndustryDescription": return "Краткое описание отрасли";
				case "offerCase": return "Кейс";
				case "offerPoint": return "Суть предложения";
				case "recommendations": return "Рекомендации";
				case "techPassport": return "Технический паспорт";
				case "certificate": return "Сертификат";
				case "coveringLetter": return "Сопроводительное письмо";
				case "similarCases": return "Аналогичные кейсы";
			}

			return "NotImplementedException";
			//throw new NotImplementedException();
		}

		#endregion Client

		private async Task CheckUserAccessAsync(int currentUserId, Offer offer)
		{
			var currentManager = await _context.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);
			if (currentManager == null)
				throw new ApiException("Не найден пользователь");
			if (!currentManager.AllowedToViewAllOffers && offer.ManagerId != currentUserId)
				throw new ApiException("Ошибка доступа", 403);
		}

		private async Task CheckUserAccess(User currentManager, Offer offer)
		{
			if (!currentManager.AllowedToViewAllOffers && offer.ManagerId != currentManager.Id)
				throw new ApiException("Ошибка доступа", 403);
		}
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

	public static class ExtensionMethods
	{
		// Deep clone
		public static T DeepClone<T>(this T a)
		{
#pragma warning disable SYSLIB0011
			using (var stream = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(stream, a);
				stream.Position = 0;
				return (T)formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011
			}
		}
	}
}