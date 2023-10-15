FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#COPY ["eda_goodline_bot.csproj", "eda_goodline_bot/"]
WORKDIR "/src/eda_goodline_bot"
COPY . .
RUN dotnet restore "eda_goodline_bot.csproj"
RUN dotnet build "eda_goodline_bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eda_goodline_bot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eda_goodline_bot.dll"]
