FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["SmartStock.Api/SmartStock.Api.csproj", "SmartStock.Api/"]
COPY ["SmartStock.Application/SmartStock.Application.csproj", "SmartStock.Application/"]
COPY ["SmartStock.Infrastructure/SmartStock.Infrastructure.csproj", "SmartStock.Infrastructure/"]
COPY ["SmartStock.Domain/SmartStock.Domain.csproj", "SmartStock.Domain/"]

RUN dotnet restore "SmartStock.Api/SmartStock.Api.csproj"

COPY . .
RUN dotnet publish "SmartStock.Api/SmartStock.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SmartStock.Api.dll"]
