# Etapa base para rodar a aplicação ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

EXPOSE 5183

# Etapa de build, utilizando o SDK do .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas o arquivo de projeto e restaura as dependências
COPY ["RabbitMQDemo/RabbitMQDemo.csproj", "RabbitMQDemo/"]
RUN dotnet restore "RabbitMQDemo/RabbitMQDemo.csproj"

# Copia todo o código fonte para o contêiner
COPY . .
WORKDIR "/src/RabbitMQDemo"

# Compila o projeto
RUN dotnet build "RabbitMQDemo.csproj" -c Release -o /app/build

# Etapa de publicação
FROM build AS publish
RUN dotnet publish "RabbitMQDemo.csproj" -c Release -o /app/publish

# Etapa final, que usa a imagem base otimizada
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Configura o ponto de entrada da aplicação
ENTRYPOINT ["dotnet", "RabbitMQDemo.dll"]
