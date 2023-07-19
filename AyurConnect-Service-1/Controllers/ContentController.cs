using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AyurConnect_Service_1.DTOs;
using AyurConnect_Service_1.Models;
using AyurConnect_Service_1.Persistence;
using System.Text.Json;
using AyurConnect_Service_1.Types;

namespace AyurConnect_Service_1.Controllers
{
    [ApiController]
    [Route("api/content")]
    public class ContentController : ControllerBase
    {
        private readonly ILogger<ContentController> _logger;

        private readonly DatabaseContext _databaseContext;

        public ContentController(ILogger<ContentController> logger, DatabaseContext context)
        {
            _logger = logger;
            _databaseContext = context;
        }

        [HttpPost]
        public async Task<ActionResult<Content>> SaveContent(ContentDto contentDto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received a content object to persist: {}", JsonSerializer.Serialize(contentDto));

            Content? content =
                await _databaseContext.Contents.FindAsync(new object?[] { contentDto.Id }, cancellationToken);

            if (content is null)
            {
                content = new Content
                {
                    Header = contentDto.Header,
                    UserId = contentDto.UserId,
                    Body = contentDto.Body,
                    ContentType = contentDto.ContentType,
                    IsDeleted = false,
                };

                _databaseContext.Contents.Add(content);
                await _databaseContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                content.Header = contentDto.Header;
                content.Body = contentDto.Body;
                content.ContentType = contentDto.ContentType;

                _databaseContext.Contents.Update(content);
                await _databaseContext.SaveChangesAsync(cancellationToken);
            }

            return Created(nameof(GetContent), content);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Content>> GetContent(long id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received a request to read a content object: {}", id);

            Content? content = await _databaseContext.Contents.FindAsync(new object?[] { id }, cancellationToken);

            if (content is null)
            {
                return NotFound();
            }

            return Ok(content);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Content>>> GetContents([FromQuery] FilterDto filterDto,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received a request to read all the contents: {}",
                JsonSerializer.Serialize(filterDto));

            IEnumerable<Content> contents;

            if (filterDto.UserId is not null)
            {
                contents = await _databaseContext.Contents
                    .Where(x => x.UserId.Equals(filterDto.UserId))
                    .OrderBy(x => x.UpdatedDate)
                    .ToListAsync(cancellationToken);
                return Ok(contents);
            }

            if (filterDto.DateSortType.Equals(SortType.ASCENDING))
            {
                contents = await _databaseContext.Contents
                    .Where(x => !x.IsDeleted && x.ContentType.Equals(filterDto.ContentType))
                    .OrderBy(x => x.UpdatedDate)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                contents = await _databaseContext.Contents
                    .Where(x => !x.IsDeleted && x.ContentType.Equals(filterDto.ContentType))
                    .OrderByDescending(x => x.UpdatedDate)
                    .ToListAsync(cancellationToken);
            }

            return Ok(contents);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContent(long id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received a content object identification to delete: {}", id);

            Content? content = await _databaseContext.Contents.FindAsync(new object?[] { id }, cancellationToken);

            if (content is null)
            {
                return NotFound();
            }

            content.IsDeleted = true;

            _databaseContext.Update(content);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}