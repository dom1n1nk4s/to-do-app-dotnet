# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY to-do-app-dotnet/*.csproj ./to-do-app-dotnet/
COPY Tests/*.csproj ./Tests/
RUN dotnet restore

# copy everything else and build app
COPY to-do-app-dotnet/. ./to-do-app-dotnet/
COPY Tests/. ./Tests/
WORKDIR /source/to-do-app-dotnet
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "to-do-app-dotnet.dll"]