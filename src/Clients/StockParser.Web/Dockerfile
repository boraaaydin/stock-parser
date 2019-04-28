FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Clients/StockParser.Web/StockParser.Web.csproj", "Clients/StockParser.Web/"]
RUN dotnet restore "Clients/StockParser.Web/StockParser.Web.csproj"
COPY . .
WORKDIR "/src/Clients/StockParser.Web"
RUN dotnet build "StockParser.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "StockParser.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "StockParser.Web.dll"]