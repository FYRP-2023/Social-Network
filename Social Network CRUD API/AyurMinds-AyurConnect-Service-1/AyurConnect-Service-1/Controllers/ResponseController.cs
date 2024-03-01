using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AyurConnect_Service_1.DTOs;
using AyurConnect_Service_1.Models;
using AyurConnect_Service_1.Persistence;
using System.Text.Json;

namespace servicecud.Controllers
{
    [ApiController]
    [Route("api/response")]
    public class ResponseController : ControllerBase
    {
        private readonly ILogger<ResponseController> _logger;

        private readonly DatabaseContext _databaseContext;

        public ResponseController(ILogger<ResponseController> logger, DatabaseContext context)
        {
            _logger = logger;
            _databaseContext = context;
        }

        [HttpPost]
        public async Task<ActionResult<Response>> SaveResponse(ResponseDto responseDto,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received a response object to persist: {}", JsonSerializer.Serialize(responseDto));

            Content? content =
                await _databaseContext.Contents.FindAsync(new object?[] { responseDto.ContentId }, cancellationToken);

            if (content is null)
            {
                return NotFound();
            }

            Response? response =
                await _databaseContext.Responses.FindAsync(new object?[] { responseDto.ResponseId }, cancellationToken);

            Response? parentResponse =
                await _databaseContext.Responses.FindAsync(new object?[] { responseDto.ParentResponseId }, cancellationToken);

            if (response is null)
            {

                response = new Response
                {
                    Body = responseDto.Body,
                    UserId = responseDto.UserId,
                    ParentResponse = parentResponse,
                    Content = content,
                    IsDeleted = false
                };

                _databaseContext.Responses.Add(response);
            }
            else
            {
                response.Body = responseDto.Body;

                _databaseContext.Responses.Update(response);
            }

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return Created(nameof(GetResponse), content);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Response>>> GetResponsesByContentId(long contentId,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Received a request to read all the responses of a content object identification: {}", contentId);

            var responses = await _databaseContext.Responses
                .Where(x => !x.IsDeleted && x.Content.Id == contentId).ToListAsync(cancellationToken);

            return Ok(responses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> GetResponse(long id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received a request to read a response object: {}", id);

            var response = await _databaseContext.Responses.FindAsync(new object?[] { id }, cancellationToken);

            if (response is null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResponse(long id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received a response object identification to delete: {}", id);

            Response? response = await _databaseContext.Responses.FindAsync(new object?[] { id }, cancellationToken);

            if (response is null)
            {
                return NotFound();
            }

            response.IsDeleted = true;

            _databaseContext.Update(response);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}