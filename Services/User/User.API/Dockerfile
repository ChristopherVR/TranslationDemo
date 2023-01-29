#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
# Restore nuget packages
COPY ["Services/User/User.API/User.API.csproj", "/src/User/User.API/"]
COPY ["Services/User/User.Domain/User.Domain.csproj", "/src/User/User.Domain/"]
COPY ["Services/User/User.Infrastructure/User.Infrastructure.csproj", "/src/User/User.Infrastructure/"]
COPY ["Services/Authentication/Authentication.csproj", "/src/Services/Authentication/"]

COPY ["Libraries/Common/API.Extensions/API.Extensions.csproj", "/src/Libraries/Common/API.Extensions/"]
COPY ["Libraries/Common/Domain.Extensions/Domain.Extensions.csproj", "/src/Libraries/Common/Domain.Extensions/"]
COPY ["Libraries/Common/Infrastructure.Extensions/Infrastructure.Extensions.csproj", "/src/Libraries/Common/Infrastructure.Extensions/"]

COPY ["Libraries/Translation/Translation.API.Engine/Translation.API.Engine.csproj", "/src/Libraries/Translation/Translation.API.Engine/"]
COPY ["Libraries/Translation/Translation.Common/Translation.Common.csproj", "/src/Libraries/Translation/Translation.Common/"]
COPY ["Libraries/Translation/Translation.Domain.Engine/Translation.Domain.Engine.csproj", "/src/Libraries/Translation/Translation.Domain.Engine/"]
COPY ["Libraries/Translation/Translation.Infrastructure.Engine/Translation.Infrastructure.Engine.csproj", "/src/Libraries/Translation/Translation.Infrastructure.Engine/"]
COPY ["Libraries/Translation/TranslationEngine/Translation.Web.Engine.csproj", "/src/Libraries/Translation/TranslationEngine/"]

RUN dotnet restore "/src/User/User.API/User.API.csproj"


# Copy source files
COPY ["Services/User/User.API/User.API.csproj", "/src/User/User.API"]
COPY ["Services/User/User.Domain/User.Domain.csproj", "/src/User/User.Domain"]
COPY ["Services/User/User.Infrastructure/User.Infrastructure.csproj", "/src/User/User.Infrastructure"]
COPY ["Services/Authentication/Authentication.csproj", "/src/Services/Authentication"]

COPY ["Libraries/Common/API.Extensions/API.Extensions.csproj", "/src/Libraries/Common/API.Extensions"]
COPY ["Libraries/Common/Domain.Extensions/Domain.Extensions.csproj", "/src/Libraries/Common/Domain.Extensions"]
COPY ["Libraries/Common/Infrastructure.Extensions/Infrastructure.Extensions.csproj", "/src/Libraries/Common/Infrastructure.Extensions"]

COPY ["Libraries/Translation/Translation.API.Engine/Translation.API.Engine.csproj", "/src/Libraries/Translation/Translation.API.Engine"]
COPY ["Libraries/Translation/Translation.Common/Translation.Common.csproj", "/src/Libraries/Translation/Translation.Common"]
COPY ["Libraries/Translation/Translation.Domain.Engine/Translation.Domain.Engine.csproj", "/src/Libraries/Translation/Translation.Domain.Engine"]
COPY ["Libraries/Translation/Translation.Infrastructure.Engine/Translation.Infrastructure.Engine.csproj", "/src/Libraries/Translation/Translation.Infrastructure.Engine"]
COPY ["Libraries/Translation/TranslationEngine/Translation.Web.Engine.csproj", "/src/Libraries/Translation/TranslationEngine"]


COPY . .
WORKDIR "/src/Services/User/User.API"
RUN dotnet build "User.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "User.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "User.API.dll"]