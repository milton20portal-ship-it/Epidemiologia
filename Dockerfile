FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Fíjate que el nombre coincida con tu archivo .csproj
COPY ["Epidemiologia.csproj", "."]
RUN dotnet restore "./Epidemiologia.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Epidemiologia.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Epidemiologia.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Epidemiologia.dll"]