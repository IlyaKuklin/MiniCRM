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
            var result = await _offersService.GetAsync(id, this.CurrentUserId);
            return this.Ok(result);
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(List<Offer.ShortDto>), 200)]
        public async Task<IActionResult> GetList(string filter)
        {
            var result = await _offersService.GetListAsync(filter, this.CurrentUserId);
            return this.Ok(result);
        }

        /// <summary>
        /// Создание / редактирование.
        /// </summary>
        /// <param name="dto">DTO. Для новых сущностей ID = -1</param>
        /// <returns></returns>
        [HttpPost("edit")]
        [ProducesResponseType(typeof(Offer.Dto), 200)]
        public async Task<IActionResult> Register([FromBody][Required] Offer.Dto dto, [Required] bool forClient)
        {
            var result = await _offersService.EditAsync(dto, forClient, this.CurrentUserId);
            return this.Ok(result);
        }

        [HttpDelete("delete")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteManager([FromQuery] int id)
        {
            await _offersService.DeleteAsync(id, this.CurrentUserId);
            return this.Ok(204);
        }

        [HttpPatch("archive_in")]
        [ProducesResponseType(201)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> MoveToArchive([Required] int id)
        {
            await _offersService.MoveToArchiveAsync(id, this.CurrentUserId);
            return this.Ok(201);
        }

        [HttpPatch("archive_out")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> MoveFromArchive([Required] int id)
        {
            await _offersService.MoveFromArchiveAsync(id, this.CurrentUserId);
            return this.Ok(201);
        }

        [HttpPatch("files/upload")]
        [ProducesResponseType(typeof(List<OfferFileDatum.Dto>), 200)]
        public async Task<IActionResult> UploadFile([Required] List<IFormFile> files, [FromQuery][Required] int offerId, [FromQuery][Required] OfferFileType type, [FromQuery][Required] bool replace)
        {
            var res = await _offersService.UploadFileAsync(files, offerId, type, replace, this.CurrentUserId);
            return this.Ok(res);
        }

        [HttpDelete("files/delete")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteFile([Required] int offerFileId)
        {
            await _offersService.DeleteFileAsync(offerFileId, this.CurrentUserId);
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

        [HttpPost("rules/edit")]
        [ProducesResponseType(typeof(OfferRule.Dto), 200)]
        public async Task<IActionResult> EditOfferRule([Required] OfferRule.Dto dto)
        {
            var result = await _offersService.EditOfferRuleAsync(dto, this.CurrentUserId);
            return this.Ok(result);
        }

        [HttpPost("rules/changeState")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ChangeOfferRuleState([Required] int ruleId)
        {
            await _offersService.ChangeOfferRuleStateAsync(ruleId, this.CurrentUserId);
            return this.Ok();
        }

        [HttpPost("rules/complete")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CompleteOfferRule([Required] OfferRule.CompleteDto dto)
        {
            await _offersService.CompleteOfferRuleAsync(dto, this.CurrentUserId);
            return this.Ok();
        }

        [HttpDelete("rules/delete")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteOfferRule([Required] int ruleId)
        {
            await _offersService.DeleteOfferRuleAsync(ruleId, this.CurrentUserId);
            return this.Ok();
        }

        [HttpGet("rules/checks/list")]
        [ProducesResponseType(typeof(List<OfferRule.Dto>), 200)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetOffersForCheck()
        {
            var result = await _offersService.GetOfferRulesForCheckAsync();
            return this.Ok(result);
        }

        [HttpPut("rules/checks/approve")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ApproveRule([Required] int ruleId)
        {
            await _offersService.ApproveOfferRuleAsync(ruleId);
            return this.Ok();
        }

        [HttpPut("rules/checks/reject")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RejectRule([Required] OfferRule.RejectDto dto)
        {
            await _offersService.RejectOfferRuleAsync(dto);
            return this.Ok();
        }

        [HttpPost("client/offer/send")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> SendOfferToClient(int offerId)
        {
            var result = await _offersService.SendOfferToClientAsync(offerId, this.CurrentUserId);
            return this.Ok(result);
        }

        [HttpGet("client/offer")]
        [ProducesResponseType(typeof(Offer.ClientViewDto), 200)]
        [AllowAnonymous]
        public async Task<IActionResult> GetOfferForClient(Guid link, string key)
        {
            var result = await _offersService.GetOfferForClientAsync(link, key, this.CurrentUserId);
            return this.Ok(result);
        }

        [HttpPost("client/offer/answerOnFeedbackRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> AnswerOnFeedbackRequest([Required] OfferFeedbackRequest.AnswerDto answerDto)
        {
            await _offersService.AnswerOnFeedbackRequestAsync(answerDto);
            return this.Ok();
        }
    }
}