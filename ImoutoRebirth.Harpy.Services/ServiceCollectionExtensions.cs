﻿using ImoutoRebirth.Common.Cqrs;
using ImoutoRebirth.Common.Quartz.Extensions;
using ImoutoRebirth.Harpy.Services.SaveFavorites.Commands;
using ImoutoRebirth.Harpy.Services.SaveFavorites.Quartz;
using ImoutoRebirth.Harpy.Services.SaveFavorites.Services;
using ImoutoRebirth.Harpy.Services.SaveFavorites.Services.Loaders;
using ImoutoRebirth.Harpy.Services.SaveFavorites.Services.Room;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImoutoRebirth.Harpy.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHarpyServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36";
        
        services.AddMediatR(typeof(FavoritesSaveCommand));
        services.AddLoggingBehavior();

        services.AddQuartzJob<FavoritesSaveJob, FavoritesSaveJob.Description>();

        services.Configure<FavoritesSaveJobSettings>(
            configuration.GetSection(nameof(FavoritesSaveJobSettings)));

        services.Configure<SaverConfiguration>(configuration.GetSection("Saver"));
        services.Configure<DanbooruBooruConfiguration>(configuration.GetSection("Danbooru"));
        services.Configure<YandereBooruConfiguration>(configuration.GetSection("Yandere"));

        services.AddHttpClient<DanbooruFavoritesLoader>(x => x.DefaultRequestHeaders.Add("User-Agent", userAgent));
        services.AddHttpClient<YandereFavoritesLoader>();
        services.AddHttpClient<RoomSavedChecker>();
        services.AddHttpClient<PostSaver>().ConfigureHttpClient(x =>
        {
            x.DefaultRequestHeaders.Add("User-Agent", userAgent);
            x.Timeout = TimeSpan.FromMinutes(5);
        });

        return services;
    }
}
