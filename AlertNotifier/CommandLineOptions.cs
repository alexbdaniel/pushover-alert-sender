using CommandLine;

namespace AlertNotifier;

public class CommandLineOptions
{
    [Option('t', "title", Required = true, HelpText = "Notification title.")]
    public required string Title { get; set; }
    
    [Option('m', "message", Required = true, HelpText = "Notification message.")]
    public required string Message { get; set; }
}