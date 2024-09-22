namespace AlertNotifier;

public class RequestArgs
{
    public required string Title { get; init; }
    
    public required string Message { get; init; }
    
    public required string Token { get; init; }
    
    public required string User { get; init; }
}