using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniCRMCore.Areas.Clients.Models;
using MiniCRMCore.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniCRMCore.Areas.Clients
{
	public class ClientsService
	{
		private readonly ApplicationContext _context;
		private readonly IMapper _mapper;

		public ClientsService(ApplicationContext context, IMapper mapper)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<Client.Dto> GetAsync(int id)
		{
			var client = await _context.Clients
				.Include(x => x.Offers)
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);
			if (client == null)
				throw new ApiException($"Не найден клиент с ID {id}");

			var dto = _mapper.Map<Client.Dto>(client);
			return dto;
		}

		public async Task<List<Client.Dto>> GetListAsync()
		{
			var clients = await _context.Clients
				.Include(x => x.Offers)
				.AsNoTracking()
				.ToListAsync();

			var dto = _mapper.Map<List<Client.Dto>>(clients);
			return dto;
		}

		public async Task<Client.Dto> EditAsync(Client.Dto dto)
		{
			Client client;
			if (dto.Id > 0)
			{
				client = await _context.Clients
					.Include(x => x.Offers)
					.FirstOrDefaultAsync(x => x.Id == dto.Id);
				if (client == null)
					throw new ApiException($"Не найден клиент с ID {dto.Id}");
			}
			else
			{
				client = new Client();
				await _context.Clients.AddAsync(client);
			}

			_mapper.Map(dto, client);
			await _context.SaveChangesAsync();

			var returnDto = _mapper.Map<Client.Dto>(client);
			return returnDto;
		}

		public async Task DeleteAsync(int id)
		{
			var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == id);
			if (client == null)
				throw new ApiException($"Не найден клиент с ID {id}");

			_context.Clients.Remove(client);
			await _context.SaveChangesAsync();
		}
	}
}