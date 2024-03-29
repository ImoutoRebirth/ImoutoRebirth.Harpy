﻿using ImoutoRebirth.Common.Host;
using ImoutoRebirth.Common.Logging;
using ImoutoRebirth.Common.Quartz.Extensions;
using GenericHost = Microsoft.Extensions.Hosting.Host;

namespace ImoutoRebirth.Harpy.Host;

internal static class Program
{
    private const string ServicePrefix = "HARPY_";

    private static async Task Main(string[] args)
    {
        await CreateHostBuilder(args)
            .Build()
            .RunAsync();
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IHostBuilder CreateHostBuilder(string[] args)
        => GenericHost.CreateDefaultBuilder(args)
            .UseWindowsService()
            .SetWorkingDirectory()
            .UseEnvironmentFromEnvironmentVariable(ServicePrefix)
            .UseConfiguration(ServicePrefix)
            .ConfigureSerilog(
                (loggerBuilder, appConfiguration)
                    => loggerBuilder
                        .WithoutDefaultLoggers()
                        .WithConsole()
                        .WithAllRollingFile()
                        .WithInformationRollingFile()
                        .PatchWithConfiguration(appConfiguration))
            .UseStartup(x => new Startup(x))
            .UseQuartz();
}
