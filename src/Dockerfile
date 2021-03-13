#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine3.12 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["/StockParser.Web/StockParser.Web.csproj", "StockParser.Web/"]
COPY ["/StockParser.Common/StockParser.Common.csproj", "StockParser.Common/"]
COPY ["/StockParser.Data/StockParser.Data.csproj", "StockParser.Data/"]
COPY ["/StockParser.Domain/StockParser.Domain.csproj", "StockParser.Domain/"]
COPY ["/StockParser.NoSql/StockParser.NoSql.csproj", "StockParser.NoSql/"]
RUN dotnet restore "/src/StockParser.Web/StockParser.Web.csproj"

COPY . . 
WORKDIR "/src/StockParser.Web"
# RUN dotnet build "StockParser.Common/StockParser.Common.csproj" -c Release -o /app/build

# RUN dotnet build "StockParser.Data/StockParser.Data.csproj" -c Release -o /app/build

# WORKDIR "StockParser/StockParser.Domain/"
# RUN dotnet build "StockParser.Domain.csproj" -c Release -o /app/build

# WORKDIR "StockParser/StockParser.NoSql/"
# RUN dotnet build "StockParser.NoSql.csproj" -c Release -o /app/build

RUN dotnet build "StockParser.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StockParser.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockParser.Web.dll"]