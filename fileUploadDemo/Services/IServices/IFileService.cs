using fileUploadDemo.Models.Dtos;

namespace fileUploadDemo.Services.IServices;

public interface IFileService
{
    Task<FileDto> UploadImageAsync(FileDtoInp input);
    Task<FileDto> UploadDocumentAsync(FileDtoInp input);
    Task<FileResultDto?> GetFileAsync(string key);
}