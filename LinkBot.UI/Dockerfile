FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["LinkBot.UI/LinkBot.UI.csproj", "LinkBot.UI/"]
RUN dotnet restore "LinkBot.UI/LinkBot.UI.csproj"
COPY . .
WORKDIR "/src/LinkBot.UI"
RUN dotnet build "LinkBot.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LinkBot.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LinkBot.UI.dll"]
