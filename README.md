# Desafio Warren

Desafio proposto pela Warren para uma vaga de desenvolvedor full stack

## Descrição

Implementar um sistema de controle de conta corrente bancária, processando solicitações de depósito, resgates e pagamentos. Um ponto extra seria rentabilizar o dinheiro parado em conta de um dia para o outro como uma conta corrente remunerada.

## Environment

Para rodar esse projeto é necessário ter instalado

- **.NET 5 SDK**
- **Docker**
- **Node.js**

## Generate backend certificate

### Powershell

```PowerShell
dotnet dev-certs https -ep $env:UserProfile\.aspnet\https\desafiowarren.pfx -p desafiowarren

dotnet dev-certs https --trust
```

### CMD

```cmd
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\desafiowarren.pfx -p desafiowarren

dotnet dev-certs https --trust
```

### Bash

```bash
export CERT_PATH=$HOME/.aspnet/https/desafiowarren.pfx

sudo dotnet dev-certs https -ep $CERT_PATH -p desafiowarren

sudo dotnet dev-certs https --trust
```

## Linux Environment

Se você irá utilizar linux para rodar o backend, primeiro deverá alterar algumas configurações no arquivo **docker-compose.override.yml**

```yml
desafiowarren.api:
  environment:
    - ASPNETCORE_ENVIRONMENT=Docker
    - ASPNETCORE_Kestrel__Certificates__Default__Password=desafiowarren
    - ASPNETCORE_Kestrel__Certificates__Default__Path=/https/desafiowarren.pfx
    - ASPNETCORE_URLS=https://+:443;http://+:80
    - TZ=America/Sao_Paulo
  ports:
    - "5000:80"
    - "5001:443"
  # replace the volumes section as shown bellow
  volumes:
    - ${HOME}/.aspnet/https:/https/
```

## Start Backend Project

```
git clone https://github.com/luizmotta01/desafio-warren.git desafio-warren

cd .\desafio-warren\

docker-compose up -d # silent mode, to see containers logs remove the '-d' parameter
```

## Start Frontend Project

```
cd .\desafio-warren-client\

npm install

npm start
```

## Azure AD Credentials

Basta me pedir :)
