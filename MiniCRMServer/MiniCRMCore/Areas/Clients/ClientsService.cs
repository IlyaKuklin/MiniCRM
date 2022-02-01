using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniCRMCore.Areas.Clients.Models;
using MiniCRMCore.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
				.Include(x => x.CommonCommunicationReports)
					.ThenInclude(x => x.Author)
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);
			if (client == null)
				throw new ApiException($"Не найден клиент с ID {id}");

			var dto = _mapper.Map<Client.Dto>(client);
			return dto;
		}

		public async Task<List<Client.Dto>> GetListAsync(string filter)
		{
			List<Client> clients;
			if (string.IsNullOrEmpty(filter))
			{
				clients = await _context.Clients
					.Include(x => x.Offers)
					.AsNoTracking()
					.ToListAsync();
			}
			else
			{
				Expression<Func<Client, bool>> predicate = x =>
					x.Name.Contains(filter) ||
					x.DomainNames.Contains(filter) ||
					x.Contact.Contains(filter) ||
					x.Diagnostics.Contains(filter) ||
					x.LegalEntitiesNames.Contains(filter)
				;

				clients = await _context.Clients
					.Include(x => x.Offers)
					.Where(predicate)
					.AsNoTracking()
					.ToListAsync();
			}

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
				client.Key = Hasher.ComputeHash("crm", Guid.NewGuid()).Replace("+","").Replace("=","");
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