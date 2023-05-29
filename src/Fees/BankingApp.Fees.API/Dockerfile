FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Fees/BankingApp.Fees.API/BankingApp.Fees.API.csproj", "src/Fees/BankingApp.Fees.API/"]
RUN dotnet restore "src/Fees/BankingApp.Fees.API/BankingApp.Fees.API.csproj"
COPY . .
WORKDIR "/src/src/Fees/BankingApp.Fees.API"
RUN dotnet build "BankingApp.Fees.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankingApp.Fees.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankingApp.Fees.API.dll"]
