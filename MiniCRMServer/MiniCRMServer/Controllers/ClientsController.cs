using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCRMCore.Areas.Auth;
using MiniCRMCore.Areas.Clients;
using MiniCRMCore.Areas.Clients.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MiniCRMServer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class ClientsController : ControllerBase
	{
		private readonly ClientsService _clientsService;

		public ClientsController(ClientsService clientsService)
		{
			_clientsService = clientsService ?? throw new ArgumentNullException(nameof(clientsService));
		}

		public int CurrentUserId => this.User.GetUserId();

		[HttpGet("")]
		[ProducesResponseType(typeof(Client.Dto), 200)]
		public async Task<IActionResult> Get([FromQuery][Required] int id)
		{
			var result = await _clientsService.GetAsync(id);
			return this.Ok(result);
		}

		[HttpGet("list")]
		[ProducesResponseType(typeof(List<Client.Dto>), 200)]
		public async Task<IActionResult> GetList(string filter)
		{
			var t = User;
			var result = await _clientsService.GetListAsync(filter);
			return this.Ok(result);
		}

		/// <summary>
		/// Создание / редактирование.
		/// </summary>
		/// <param name="dto">DTO. Для новых сущностей ID = -1</param>
		/// <returns></returns>
		[HttpPost("edit")]
		[ProducesResponseType(typeof(Client.Dto), 200)]
		public async Task<IActionResult> Edit([FromBody][Required] Client.Dto dto)
		{
			var result = await _clientsService.EditAsync(dto);
			return this.Ok(result);
		}

		[HttpDelete("delete")]
		[ProducesResponseType(204)]
		public async Task<IActionResult> DeleteManager([FromQuery] int id)
		{
			await _clientsService.DeleteAsync(id);
			return this.Ok(204);
		}
	}
}