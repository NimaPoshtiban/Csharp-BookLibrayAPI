
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BookLibrary/BookLibrary.csproj", "BookLibrary/"]
RUN dotnet restore "BookLibrary/BookLibrary.csproj"
COPY . .
WORKDIR "/src/BookLibrary"
RUN dotnet build "BookLibrary.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookLibrary.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookLibrary.dll"]