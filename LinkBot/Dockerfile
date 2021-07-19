FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["LinkBot.csproj", "LinkBot/"]
RUN dotnet restore "LinkBot.csproj"
COPY . .
WORKDIR "/src/LinkBot"
RUN dotnet build "LinkBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LinkBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LinkBot.dll"]
