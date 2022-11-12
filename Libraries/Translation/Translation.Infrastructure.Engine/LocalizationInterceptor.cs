using AuthenticationService.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

            foreach (Microsoft.EntityFrameworkCore.Metadata.IProperty? property in materializationData.EntityType
                .GetProperties()
                .Where(y => y.FindAnnotation(nameof(LocalizedAttribute)) is not null))
            {
                if (property.GetAnnotation(nameof(LocalizedAttribute)).Value is not LocalizedAttribute attribute)
                {
                    logger.LogWarning("Unable to find attribute for the shadow property.");
                }
                else
                {
                    var data = actualType.GetProperty(property.Name)!.GetValue(entity) as IList<LocalizedString>;
                    foreach (LocalizedString record in data!)
                    {
                        // entity._fields.Add(new()
                        // {
                        //     Culture = record.Culture,
                        //     Field = property.Name,
                        //     Value = record.Value,
                        // });
                    }
                    actualType.GetProperty(property.Name)!.SetValue(entity, entity.GetValue(property.Name, useFallback: true));
                }
            }
        }

        return instance;
    }
}
