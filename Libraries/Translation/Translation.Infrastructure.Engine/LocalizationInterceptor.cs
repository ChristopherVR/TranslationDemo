using AuthenticationService.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using Translation.Domain.Engine.SeedWork;
using Translation.Infrastructure.Engine.Annotations;
using Translation.Infrastructure.Engine.Options;
using Translation.Infrastructure.Engine.Services;

namespace Translation.Infrastructure.Engine;

internal sealed class LocalizationInterceptor : IMaterializationInterceptor
{
    public async Task<object> InitializedInstance(MaterializationInterceptionData materializationData, object instance)
    {
        if (instance is LocalizedEntity entity)
        {
            var actualType = entity.GetType();

            var logger = materializationData.Context
                .GetService<ILoggerFactory>()
                .CreateLogger($"{nameof(LocalizationInterceptor)}Logger");

            string? siteCulture = await materializationData.Context
                ?.GetService<ISiteService>()
                ?.GetSiteCultureAsync()!;

            var userCulture = materializationData.Context
                .GetService<IUserService>()
                ?.GetUserCulture();

            var fallbackCulture = materializationData.Context
                .GetService<IOptions<LocalizationOptions>>()
                ?.Value?.DefaultCulture ?? LocalizationOptions.FallbackCulture;

            entity.SiteCulture = siteCulture;
            entity.FallbackCulture = fallbackCulture;
            entity.UserCulture = userCulture;

            foreach(var property in materializationData.EntityType
                .GetProperties()
                .Where(y => y.FindAnnotation(nameof(LocalizedAttribute)) is not null))
            {
                var shadowData = materializationData.EntityType.FindProperty($"{property.Name}_Data");

                if (shadowData is null)
                {
                    logger.LogWarning("Unable to find shadow data for this property.");
                }
                else
                {
                    var data = actualType.GetProperty(shadowData.Name)!.GetValue(entity) as IList<LocalizedField>;

                    foreach(var record in data!)
                    {
                        entity._fields.Add(record);
                    }
                    actualType.GetProperty(property.Name)!.SetValue(entity, entity.GetValue(property.Name, useFallback: true));
                }
            }
        }

        return instance;
    }
}
