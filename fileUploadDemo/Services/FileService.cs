using fileUploadDemo.Models.Dtos;
using fileUploadDemo.Models.Enums;
using fileUploadDemo.Services.IServices;

namespace fileUploadDemo.Services;

public class FileService: IFileService
{
    private readonly string _uploadsFolder;

    public FileService(IWebHostEnvironment env)
    {
        _uploadsFolder = Path.Combine(env.ContentRootPath, "Uploads");
    }
    
    public async Task<FileDto> UploadImageAsync(FileDtoInp input)
    {
        
        var allowedExtensions = new[] { ".jpg", ".png" };
        var extension = Path.GetExtension(input.File.FileName);
        
        if (!allowedExtensions.Contains(extension))
        {
            var extensionsWithoutDots = allowedExtensions.Select(ext => ext.TrimStart('.'));
            throw new Exception($"Invalid file type. Allowed types are: {string.Join(", ", extensionsWithoutDots)}");
        }
        
        if (input.File == null || input.File.Length == 0)
            throw new Exception("No file uploaded.");
    
        if (!Directory.Exists(_uploadsFolder))
            Directory.CreateDirectory(_uploadsFolder);

        var fileName = $"{ GetFileName(input.Type) }{extension}";
        
        var filePath = Path.Combine(_uploadsFolder, fileName);
    
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await input.File.CopyToAsync(stream);
        }

        return new 
            FileDto()
            {
                FileName = fileName, 
                Length = input.File.Length
            };
    }

    public async Task<FileDto> UploadDocumentAsync(FileDtoInp input)
    {
        var allowedExtensions = new[] { ".pdf", ".txt" };
        var extension = Path.GetExtension(input.File.FileName);
        
        if (!allowedExtensions.Contains(extension))
        {
            var extensionsWithoutDots = allowedExtensions.Select(ext => ext.TrimStart('.'));
            throw new Exception($"Invalid file type. Allowed types are: {string.Join(", ", extensionsWithoutDots)}");
        }
        
        if (input.File == null || input.File.Length == 0)
            throw new Exception("No file uploaded.");
    
        if (!Directory.Exists(_uploadsFolder))
            Directory.CreateDirectory(_uploadsFolder);
    
        var fileName = $"{ GetFileName(input.Type) }{extension}";
        var filePath = Path.Combine(_uploadsFolder, fileName);
    
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await input.File.CopyToAsync(stream);
        }

        return new 
            FileDto()
            {
                FileName = fileName, 
                Length = input.File.Length
            };
    }

    public async Task<FileResultDto?> GetFileAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key) || key.Contains(".."))
            return null;

        var filePath = Path.Combine(_uploadsFolder, key);

        if (!System.IO.File.Exists(filePath))
            return null;

        var extension = Path.GetExtension(filePath).ToLower();

        var contentType = extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".pdf" => "application/pdf",
            _ => "application/octet-stream"
        };

        var bytes = await System.IO.File.ReadAllBytesAsync(filePath);

        return new FileResultDto
        {
            Content = bytes,
            ContentType = contentType,
            FileName = key
        };
    }

    private string GetFileName(FileTypeEnum input)
        => input switch
        {
            FileTypeEnum.ProfilePicture => $"PRF-{DateTime.UtcNow:yyMMddHHmmss}",
            FileTypeEnum.PostImage => $"PST-{DateTime.UtcNow:yyMMddHHmmss}",
            FileTypeEnum.Resume => $"RSM-{DateTime.UtcNow:yyMMddHHmmss}",
            _ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
        };
}