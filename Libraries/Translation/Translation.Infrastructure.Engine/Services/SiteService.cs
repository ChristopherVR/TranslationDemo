using AuthenticationService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translation.Infrastructure.Engine.Options;

namespace Translation.Infrastructure.Engine.Services;

internal class SiteService : ISiteService
{
	private Dictionary<int, string> SiteCultures { get; init; } = new();
    private readonly IUserService _userService;
	private readonly SiteOptions _siteOptions;
	private readonly LocalizationOptions _localizationOptions;
	private readonly string _connectionString;

    public SiteService(
		IUserService userService, 
		IOptions<SiteOptions> siteOptions,
		IOptions<LocalizationOptions> localizationOptions,
		IConfiguration options)
	{
		_userService = userService ?? throw new ArgumentNullException(nameof(userService));
		_siteOptions = siteOptions?.Value ?? throw new ArgumentNullException(nameof(siteOptions));
        _localizationOptions = localizationOptions?.Value ?? throw new ArgumentNullException(nameof(localizationOptions));
		_connectionString = options?.GetConnectionString("Default") ?? throw new ArgumentNullException(nameof(options));
    }

	public async Task<string?> GetSiteCultureAsync()
	{
		string? culture = _localizationOptions.DefaultCulture ?? LocalizationOptions.FallbackCulture;

		int? siteId = _userService.GetSiteId();

		siteId ??= _siteOptions.DefaultSiteId;

		if (siteId is null)
		{
			return culture;
		}

		if (SiteCultures.TryGetValue(siteId!.Value, out culture))
		{
			return culture;
		}

		var sql = $@"
					SELECT
							Culture
					FROM [].[Sites] WITH(NOLOCK) S
					WHERE [S].[Id] = @siteId";
		// TODO: complete this query.

		_ = SiteCultures.TryAdd(siteId!.Value, null!);
		await Task.CompletedTask;
		return null;
	}
}
