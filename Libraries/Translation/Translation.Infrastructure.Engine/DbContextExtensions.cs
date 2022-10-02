using Microsoft.EntityFrameworkCore;

namespace Translation.Infrastructure.Engine;

public static class DbContextExtensions
{
    private static readonly LocalizationInterceptor _localizationInterceptor = new();

    public static DbContextOptionsBuilder AddLocalizationInterceptor(this DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .AddInterceptors(_localizationInterceptor);
}