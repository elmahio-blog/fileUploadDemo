namespace fileUploadDemo.Models.Dtos;

public class FileResultDto
{
    public byte[] Content { get; set; } = default!;
    public string ContentType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}