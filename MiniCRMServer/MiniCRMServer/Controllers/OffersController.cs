using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCRMCore.Areas.Offers;
using MiniCRMCore.Areas.Offers.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MiniCRMServer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class OffersController : ControllerBase
	{
		private readonly OffersService _offersService;

		public OffersController(OffersService offersService)
		{
			_offersService = offersService ?? throw new ArgumentNullException(nameof(offersService));
		}

		[HttpGet("")]
		[ProducesResponseType(typeof(Offer.Dto), 200)]
		public async Task<IActionResult> Get([FromQuery][Required] int id)
		{
			var result = await _offersService.GetAsync(id);
			return this.Ok(result);
		}

		[HttpGet("list")]
		[ProducesResponseType(typeof(List<Offer.Dto>), 200)]
		public async Task<IActionResult> GetList()
		{
			var result = await _offersService.GetListAsync();
			return this.Ok(result);
		}

		/// <summary>
		/// Создание / редактирование.
		/// </summary>
		/// <param name="dto">DTO. Для новых сущностей ID = -1</param>
		/// <returns></returns>
		[HttpPost("edit")]
		[ProducesResponseType(typeof(Offer.Dto), 200)]
		public async Task<IActionResult> Register([FromBody][Required] Offer.Dto dto)
		{
			var result = await _offersService.EditAsync(dto);
			return this.Ok(result);
		}

		[HttpDelete("delete")]
		[ProducesResponseType(204)]
		public async Task<IActionResult> DeleteManager([FromQuery] int id)
		{
			await _offersService.DeleteAsync(id);
			return this.Ok(204);
		}
	}
}