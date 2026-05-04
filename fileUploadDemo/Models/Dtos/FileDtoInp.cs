using fileUploadDemo.Models.Enums;

namespace fileUploadDemo.Models.Dtos;

public class FileDtoInp
{
    public FileTypeEnum Type { get; set; }
    public IFormFile File { get; set; }
}