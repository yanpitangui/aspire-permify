##See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080

# Install cultures (same approach as Alpine SDK image)
RUN apk add --no-cache icu-libs

# Disable the invariant mode (set in base image)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["./Directory.Packages.props", "./"]
COPY ["src/Permify.Api/Permify.Api.csproj", "src/Permify.Api/"]
COPY ["src/Permify.Application/Permify.Application.csproj", "src/Permify.Application/"]
COPY ["src/Permify.Domain/Permify.Domain.csproj", "src/Permify.Domain/"]
COPY ["src/Permify.Infrastructure/Permify.Infrastructure.csproj", "src/Permify.Infrastructure/"]
COPY . .
WORKDIR "/src/src/Permify.Api"

FROM build AS publish
RUN dotnet publish "Permify.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Permify.Api.dll"]