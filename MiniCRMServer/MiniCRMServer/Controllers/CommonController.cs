using Microsoft.AspNetCore.Mvc;
using MiniCRMCore.Areas.Auth;
using MiniCRMCore.Areas.Common;
using MiniCRMCore.Areas.Email.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MiniCRMServer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommonController : ControllerBase
	{
		private readonly CommonService _commonService;

		public CommonController(CommonService commonService)
		{
			_commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
		}

		public int CurrentUserId => this.User.GetUserId();

		#region CommunicationReports

		[HttpPost("communicationReports/edit")]
		[ProducesResponseType(typeof(CommunicationReport.Dto), 201)]
		public async Task<IActionResult> EditCommunicationReport([Required] CommunicationReport.EditDto dto)
		{
			var result = await _commonService.EditCommunicationReportAsync(dto, this.CurrentUserId);
			return this.Ok(result);
		}

		[HttpDelete("communicationReports/delete")]
		[ProducesResponseType(201)]
		public async Task<IActionResult> DeleteCommunicationReport([FromQuery][Required] int id)
		{
			await _commonService.DeleteCommunicationReportAsync(id);
			return this.Ok(204);
		}

		#endregion CommunicationReports

		[HttpPost("settings/update")]
		[ProducesResponseType(200)]
		public async Task<IActionResult> UpdateSettings([Required] EmailSettings.Dto dto)
		{
			await _commonService.UpdateEmailSettingsAsync(dto);
			return this.Ok();
		}

		[HttpGet("settings/get")]
		[ProducesResponseType(typeof(EmailSettings.Dto), 200)]
		public async Task<IActionResult> GetSettings()
		{
			var result = await _commonService.GetSettingsAsync();
			return this.Ok(result);
		}
	}
}