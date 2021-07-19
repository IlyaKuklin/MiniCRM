using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniCRMCore.Areas.Offers.Models;
using MiniCRMCore.Utilities.Exceptions;
using System;
using System.Collections.Generic;
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

		public async Task<Offer.Dto> EditAsync(Offer.EditDto dto)
		{
			Offer offer;
			if (dto.Id > 0)
			{
				offer = await _context.Offers.FirstOrDefaultAsync(x => x.Id == dto.Id);
				if (offer == null)
					throw new ApiException($"Не найдено КП с ID {dto.Id}");
			}
			else
			{
				offer = new Offer();
				await _context.Offers.AddAsync(offer);
			}

			_mapper.Map(dto, offer);

			//var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == dto.ClientId);
			//if (client == null)
			//	throw new ApiException($"Не найден клиент с ID {dto.ClientId}");

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
	}
}