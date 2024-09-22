using System.Reflection;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AlertNotifier;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var result = Parser.Default.ParseArguments<CommandLineOptions>(args);

        await result.WithParsedAsync(RunOptionsAsync);

        result.WithNotParsed(HandleParseError);
  
        Console.WriteLine($"Exit code= {Environment.ExitCode}");

    }

    private static async Task RunOptionsAsync(CommandLineOptions cliOptions)
    {
        
        var builder = Host.CreateApplicationBuilder();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
            .AddEnvironmentVariables()
            .Build();
        
        builder.Configuration.AddConfiguration(configuration);

        var services = builder.Services;

        services.AddOptions<SenderOptions>().Bind(configuration.GetSection(SenderOptions.Key));

        var host = builder.Build();
        
        var provider = services.BuildServiceProvider(new ServiceProviderOptions()
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        });


        var senderOptions = provider.GetRequiredService<IOptions<SenderOptions>>();

        var requestArgs = new RequestArgs
        {
            Title = cliOptions.Title,
            Message = cliOptions.Message,
            Token = senderOptions.Value.BearerToken,
            User = senderOptions.Value.UserId
        };
        
        await new Sender(senderOptions).SendAsync(requestArgs);

    }

    private static void HandleParseError(IEnumerable<Error> errors)
    {
        foreach (var error in errors)
        {
            Console.WriteLine($"{error}");
        }
    }
}