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

		public async Task<Offer.Dto> GetAsync(int id)
		{
			var client = await _context.Offers
				.Include(x => x.Client)
				.Include(x => x.FileData)
					.ThenInclude(x => x.FileDatum)
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);
			if (client == null)
				throw new ApiException($"Не найден клиент с ID {id}");

			var dto = _mapper.Map<Offer.Dto>(client);
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
	}
}