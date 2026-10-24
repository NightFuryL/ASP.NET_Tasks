namespace AspNetAssignments.DTOs.Common;

public class FileResponseDTO
{
    public string RelativePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
}
