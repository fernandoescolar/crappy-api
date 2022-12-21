FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.17 AS base
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ./src/*.csproj .
RUN dotnet restore
COPY ./src/. .

FROM build AS publish
WORKDIR /src
RUN dotnet publish -c Release -o /app --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

RUN addgroup --group dotnetgroup --gid 2000  && adduser --uid 1000 --gid 2000 dotnetuser
RUN chown dotnetuser:dotnetgroup  /app
USER dotnetuser:dotnetgroup

ENTRYPOINT ["dotnet", "CrappyApi.dll"]
