namespace AlertNotifier;

public class SenderOptions
{
    public const string Key = "Http";
    
    public required string BaseAddress { get; init; }
    
    public required string Endpoint { get; init; }
    
    public required string BearerToken { get; init; }
    
    public required string UserId { get; init; }
    
}