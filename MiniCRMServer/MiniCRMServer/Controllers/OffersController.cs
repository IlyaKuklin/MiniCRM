using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniCRMCore.Areas.Auth;
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

		public int CurrentUserId => this.User.GetUserId();

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
			var result = await _offersService.EditAsync(dto, this.CurrentUserId);
			return this.Ok(result);
		}

		[HttpDelete("delete")]
		[ProducesResponseType(204)]
		public async Task<IActionResult> DeleteManager([FromQuery] int id)
		{
			await _offersService.DeleteAsync(id);
			return this.Ok(204);
		}

		[HttpPatch("files/upload")]
		[ProducesResponseType(typeof(List<OfferFileDatum.Dto>), 200)]
		public async Task<IActionResult> UploadFile([Required] List<IFormFile> files, [FromQuery][Required] int offerId, [FromQuery][Required] OfferFileType type, [FromQuery][Required] bool replace)
		{
			var res = await _offersService.UploadFileAsync(files, offerId, type, replace);
			return this.Ok(res);
		}

		[HttpDelete("files/delete")]
		[ProducesResponseType(204)]
		public async Task<IActionResult> DeleteFile([Required] int offerFileId)
		{
			await _offersService.DeleteFileAsync(offerFileId);
			return this.Ok(204);
		}

		[HttpPost("newsbreaks/add")]
		[ProducesResponseType(typeof(OfferNewsbreak.Dto), 200)]
		public async Task<IActionResult> AddOfferNewsbreak([Required] OfferNewsbreak.AddDto addDto)
		{
			var result = await _offersService.AddOfferNewsbreakAsync(addDto, this.CurrentUserId);
			return this.Ok(result);
		}

		[HttpPost("feedbackRequests/add")]
		[ProducesResponseType(typeof(OfferFeedbackRequest.Dto), 200)]
		public async Task<IActionResult> AddOfferFeedbackRequest([Required] OfferFeedbackRequest.AddDto addDto)
		{
			var result = await _offersService.AddOfferFeedbackRequestAsync(addDto, this.CurrentUserId);
			return this.Ok(result);
		}

		[HttpGet("client/offer")]
		[ProducesResponseType(typeof(Offer.ClientViewDto), 200)]
		public async Task<IActionResult> GetOfferForClient(Guid link, string key)
		{
			var result = await _offersService.GetOfferForClientAsync(link, key);
			return this.Ok(result);
		}
	}
}