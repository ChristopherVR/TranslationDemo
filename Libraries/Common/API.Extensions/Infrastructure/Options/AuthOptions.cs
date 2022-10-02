﻿namespace API.Extensions.Infrastructure.Options;

public class AuthOptions
{
    public const string Name = "Auth";

    public string Authority { get; set; } = "http://localhost";
    public required string Key { get; set; }
}
