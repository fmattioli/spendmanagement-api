#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Spents.API/Spents.API.csproj", "Spents.API/"]
COPY ["Spents.Infra.IOC/Spents.Infra.CrossCutting.csproj", "Spents.Infra.IOC/"]
COPY ["Spents.Application/Spents.Application.csproj", "Spents.Application/"]
COPY ["Spents.Infra.Data/Spents.Infra.Data.csproj", "Spents.Infra.Data/"]
COPY ["Spents.Domain/Spents.Domain.csproj", "Spents.Domain/"]
RUN dotnet restore "Spents.API/Spents.API.csproj"
COPY . .
WORKDIR "/src/Spents.API"
RUN dotnet build "Spents.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spents.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Spents.API.dll"]