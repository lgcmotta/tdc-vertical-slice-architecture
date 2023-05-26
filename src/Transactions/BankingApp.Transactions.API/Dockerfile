FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Transactions/BankingApp.Transactions.API/BankingApp.Transactions.API.csproj", "src/Transactions/BankingApp.Transactions.API/"]
RUN dotnet restore "src/Transactions/BankingApp.Transactions.API/BankingApp.Transactions.API.csproj"
COPY . .
WORKDIR "/src/src/Transactions/BankingApp.Transactions.API"
RUN dotnet build "BankingApp.Transactions.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankingApp.Transactions.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankingApp.Transactions.API.dll"]
