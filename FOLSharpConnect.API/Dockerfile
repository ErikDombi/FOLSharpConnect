FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000

RUN apt-get update && \
    apt-get install -yq tzdata && \
    ln -fs /usr/share/zoneinfo/America/New_York /etc/localtime && \
    dpkg-reconfigure -f noninteractive tzdata

ENV TZ="America/New_York"

RUN apt-get update
RUN DEBIAN_FRONTEND=noninteractive apt-get install -y wget unzip xorg openbox fontconfig locales gconf-service libasound2 libatk1.0-0 libc6 libcairo2 libcups2 libdbus-1-3 libexpat1 libfontconfig1 libgcc1 libgconf-2-4 libgdk-pixbuf2.0-0 libglib2.0-0 libgtk-3-0 libnspr4 libpango-1.0-0 libpangocairo-1.0-0 libstdc++6 libx11-6 libx11-xcb1 libxcb1 libxcomposite1 libxcursor1 libxdamage1 libxext6 libxfixes3 libxi6 libxrandr2 libxrender1 libxss1 libxtst6 ca-certificates fonts-liberation libappindicator1 libnss3 lsb-release xdg-utils wget

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FOLSharpConnect.API/FOLSharpConnect.API.csproj", "FOLSharpConnect.API/"]
RUN dotnet restore "FOLSharpConnect.API/FOLSharpConnect.API.csproj"
COPY . .
WORKDIR "/src/FOLSharpConnect.API"
RUN dotnet build "FOLSharpConnect.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FOLSharpConnect.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "FOLSharpConnect.API.dll"]
