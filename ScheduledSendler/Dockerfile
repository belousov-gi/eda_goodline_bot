FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ScheduledSendler/ScheduledSendler.csproj", "ScheduledSendler/"]
RUN dotnet restore "ScheduledSendler/ScheduledSendler.csproj"
COPY . .
WORKDIR "/src/ScheduledSendler"
RUN dotnet build "ScheduledSendler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScheduledSendler.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScheduledSendler.dll"]
