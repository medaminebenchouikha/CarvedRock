FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV SimpleProperty="hello-from-FINAL-dockerfile"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CarvedRock.App/CarvedRock.App.csproj", "CarvedRock.App/"]
RUN dotnet restore "./CarvedRock.App/./CarvedRock.App.csproj"
COPY . .
WORKDIR "/src/CarvedRock.App"
RUN dotnet build "./CarvedRock.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarvedRock.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV SimpleProperty="hello-from-FINAL-dockerfile"
ENTRYPOINT ["dotnet", "CarvedRock.App.dll"]