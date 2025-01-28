FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="12345"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/https/aspnetcore-dev.pfx"

RUN mkdir /https
RUN chown -R app:app /https

USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GenerateShortUrl.API/GenerateShortUrl.API.csproj", "GenerateShortUrl.API/"]
COPY ["GenerateShortUrls.BLL/GenerateShortUrls.BLL.csproj", "GenerateShortUrls.BLL/"]
COPY ["GenerateShortUrsl.Data/GenerateShortUrsl.Data.csproj", "GenerateShortUrsl.Data/"]

RUN dotnet restore "./GenerateShortUrl.API/./GenerateShortUrl.API.csproj"
COPY . . 
WORKDIR "/src/GenerateShortUrl.API"
RUN dotnet build "./GenerateShortUrl.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "./GenerateShortUrl.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 
COPY aspnetcore-dev.pfx /https/aspnetcore-dev.pfx
RUN chown app:app /https/aspnetcore-dev.pfx
ENTRYPOINT ["dotnet", "GenerateShortUrl.API.dll"]