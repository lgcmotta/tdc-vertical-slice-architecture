FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Gateway/BankingApp.Gateway/BankingApp.Gateway.csproj", "src/Gateway/BankingApp.Gateway/"]
RUN dotnet restore "src/Gateway/BankingApp.Gateway/BankingApp.Gateway.csproj"
COPY . .
WORKDIR "/src/src/Gateway/BankingApp.Gateway"
RUN dotnet build "BankingApp.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankingApp.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankingApp.Gateway.dll"]
