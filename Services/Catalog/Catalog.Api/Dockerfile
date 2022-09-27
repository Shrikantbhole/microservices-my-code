#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# Base asp.net files needed at runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
# Command for Build - It will use image of microsoft sdk
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Catalog/Catalog.Api/Catalog.Api.csproj", "Services/Catalog/Catalog.Api/"]
RUN dotnet restore "Services/Catalog/Catalog.Api/Catalog.Api.csproj"
COPY . .
WORKDIR "/src/Services/Catalog/Catalog.Api"
RUN dotnet build "Catalog.Api.csproj" -c Release -o /app/build
# Command for publish - It will add on build command
FROM build AS publish
RUN dotnet publish "Catalog.Api.csproj" -c Release -o /app/publish
# Command for deployment - It will contain publish folder + asp net bundles
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]
