using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniCRMCore.Areas.Email.Models;
using MiniCRMCore.Utilities.Exceptions;
using System;
using System.Threading.Tasks;

namespace MiniCRMCore.Areas.Common
{
	public class CommonService
	{
		private readonly ApplicationContext _context;
		private readonly IMapper _mapper;

		public CommonService(ApplicationContext context, IMapper mapper)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		#region CommunicationReports

		public async Task<CommunicationReport.Dto> EditCommunicationReportAsync(CommunicationReport.EditDto dto, int currentUserId)
		{
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);
			if (user == null)
				throw new ApiException($"Не найден пользователь с ID {currentUserId}");

			CommunicationReport communicationReport;
			if (dto.Id > 0)
			{
				communicationReport = await _context.CommunicationReports
					.Include(x => x.Author)
					.FirstOrDefaultAsync(x => x.Id == dto.Id);
				if (communicationReport == null)
					throw new ApiException($"Не найден отчёт с ID {dto.Id}");
			}
			else
			{
				communicationReport = new CommunicationReport();
				//await _context.CommunicationReports.AddAsync(communicationReport);
				if (dto.ClientId > -1)
				{
					var client = await _context.Clients
						.Include(x => x.CommonCommunicationReports)
						.FirstOrDefaultAsync(x => x.Id == dto.ClientId);
					client.CommonCommunicationReports.Add(communicationReport);
				}
				else if (dto.OfferId > -1)
				{
					var offer = await _context.Offers
						.Include(x => x.CommonCommunicationReports)
						.FirstOrDefaultAsync(x => x.Id == dto.OfferId);
					offer.CommonCommunicationReports.Add(communicationReport);
				}
			}

			_mapper.Map(dto, communicationReport);
			communicationReport.AuthorId = user.Id;

			await _context.SaveChangesAsync();

			var returnDto = _mapper.Map<CommunicationReport.Dto>(communicationReport);
			return returnDto;
		}

		public async Task DeleteCommunicationReportAsync(int id)
		{
			var report = await _context.CommunicationReports.FirstOrDefaultAsync(x => x.Id == id);
			if (report == null)
				throw new ApiException($"Не найден отчёт с ID {id}");

			_context.CommunicationReports.Remove(report);
			await _context.SaveChangesAsync();
		}

		#endregion CommunicationReports

		public async Task UpdateEmailSettingsAsync(EmailSettings.Dto dto)
		{
			var settings = await _context.EmailSettings.FirstOrDefaultAsync();
			if (settings == null)
			{
				settings = new EmailSettings();
				_context.EmailSettings.Add(settings);
			}

			_mapper.Map(dto, settings);
			await _context.SaveChangesAsync();
		}

		public async Task<EmailSettings.Dto> GetSettingsAsync()
		{
			var settings = await _context.EmailSettings.FirstOrDefaultAsync();
			return _mapper.Map<EmailSettings.Dto>(settings);
		}
	}
}