#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y curl
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/SpendManagement.API/SpendManagement.API.csproj", "SpendManagement.API/"]
COPY ["src/SpendManagement.Infra.IOC/SpendManagement.Infra.CrossCutting.csproj", "SpendManagement.Infra.IOC/"]
COPY ["src/SpendManagement.Application/SpendManagement.Application.csproj", "SpendManagement.Application/"]
COPY ["src/SpendManagement.Client/SpendManagement.Client.csproj", "SpendManagement.Client/"]
RUN dotnet restore "SpendManagement.API/SpendManagement.API.csproj"
COPY . .

RUN dotnet build "src/SpendManagement.API/SpendManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/SpendManagement.API/SpendManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpendManagement.API.dll"]