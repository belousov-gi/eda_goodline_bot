FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#COPY ["ScheduledSendler.csproj", "ScheduledSendler/"]
WORKDIR "/src/ScheduledSendler"
COPY . .
RUN dotnet restore "ScheduledSendler.csproj"
RUN dotnet build "ScheduledSendler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScheduledSendler.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN apt-get update && apt-get -y install cron
COPY --from=publish /app/publish .
COPY crontask /etc/cron.d
RUN chmod 0644 /etc/cron.d/crontask
CMD ["cron", "-f"]
