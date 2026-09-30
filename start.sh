#!/usr/bin/env bash
set -euo pipefail

export ASPNETCORE_ENVIRONMENT="${ASPNETCORE_ENVIRONMENT:-Production}"
export ASPNETCORE_URLS="${ASPNETCORE_URLS:-http://0.0.0.0:${PORT:-8080}}"

cd /app

dotnet restore SmartStockBackend.sln
DOTNET_CLI_TELEMETRY_OPTOUT=1 dotnet publish SmartStock.Api/SmartStock.Api.csproj -c Release -o ./publish

exec dotnet ./publish/SmartStock.Api.dll
