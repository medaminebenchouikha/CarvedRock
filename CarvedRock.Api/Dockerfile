FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV SimpleProperty="hello-from-FINAL-dockerfile"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CarvedRock.Api/CarvedRock.Api.csproj", "CarvedRock.Api/"]
RUN dotnet restore "./CarvedRock.Api/./CarvedRock.Api.csproj"
COPY . .
WORKDIR "/src/CarvedRock.Api"
RUN dotnet build "./CarvedRock.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarvedRock.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV SimpleProperty="hello-from-FINAL-dockerfile"
ENTRYPOINT ["dotnet", "CarvedRock.Api.dll"]