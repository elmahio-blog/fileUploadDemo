using fileUploadDemo.Models.Dtos;
using fileUploadDemo.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace fileUploadDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController: ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");
    
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
    
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);
    
        var filePath = Path.Combine(uploadsFolder, file.FileName);
    
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
    
        return Ok(new { file.FileName, file.Length });
    }
    
    [HttpPost("UploadImage")]
    public async Task<IActionResult> UploadImageAsync([FromForm] FileDtoInp input)
    {
        var result = await _fileService.UploadImageAsync(input);
        return Ok(result);
    }
    
    [HttpPost("UploadDocument")]
    public async Task<IActionResult> UploadDocumentAsync([FromForm] FileDtoInp input)
    {
        var result = await _fileService.UploadDocumentAsync(input);
        return Ok(result);
    }
    
    [HttpGet("GetFile")]
    public async Task<IActionResult> GetFileAsync([FromQuery] string key)
    {
        var result = await _fileService.GetFileAsync(key);

        if (result == null)
            return NotFound("File not found");

        return File(result.Content, result.ContentType, result.FileName);
    }
}