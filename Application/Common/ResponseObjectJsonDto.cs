namespace Application.Common;

public class ResponseObjectJsonDto
{
    [JsonPropertyName("response")]
    public object? Response {  get; set; }
        
    [JsonPropertyName("code")]
    public int Code { get; set; }
        
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}