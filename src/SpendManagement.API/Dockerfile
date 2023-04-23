#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SpendManagement.API/SpendManagement.API.csproj", "SpendManagement.API/"]
COPY ["SpendManagement.Infra.IOC/SpendManagement.Infra.CrossCutting.csproj", "SpendManagement.Infra.IOC/"]
COPY ["SpendManagement.Application/SpendManagement.Application.csproj", "SpendManagement.Application/"]
COPY ["SpendManagement.Infra.Data/SpendManagement.Infra.Data.csproj", "SpendManagement.Infra.Data/"]
COPY ["SpendManagement.Domain/SpendManagement.Domain.csproj", "SpendManagement.Domain/"]
RUN dotnet restore "SpendManagement.API/SpendManagement.API.csproj"
COPY . .
WORKDIR "/src/SpendManagement.API"
RUN dotnet build "SpendManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpendManagement.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpendManagement.API.dll"]