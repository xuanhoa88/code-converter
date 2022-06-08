FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 40163
ENV ASPNETCORE_URLS=http://+:40163

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./Api/Api.csproj" --disable-parallel
RUN dotnet publish "./Api/Api.csproj" -c release -o /app --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ICSharpCode.CodeConverter.Api.dll"]