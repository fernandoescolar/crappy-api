FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.17 AS base
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

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

RUN apk add --no-cache icu-libs
RUN addgroup app
RUN adduser -D app -G app
USER app

ENTRYPOINT ["dotnet", "CrappyApi.dll"]
