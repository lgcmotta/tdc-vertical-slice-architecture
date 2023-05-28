FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Accounts/BankingApp.Accounts.API/BankingApp.Accounts.API.csproj", "src/Accounts/BankingApp.Accounts.API/"]
RUN dotnet restore "src/Accounts/BankingApp.Accounts.API/BankingApp.Accounts.API.csproj"
COPY . .
WORKDIR "/src/src/Accounts/BankingApp.Accounts.API"
RUN dotnet build "BankingApp.Accounts.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankingApp.Accounts.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankingApp.Accounts.API.dll"]
