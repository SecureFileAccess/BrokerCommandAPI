namespace Models;

public class CommandRequest
{
    public required string ClientId { get; set; }
    public required string Action { get; set; }
    public required string FilePath { get; set; }
    public string? Content { get; set; }
}
