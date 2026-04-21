FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Kakushkin_NewsFeed.Host/Kakushkin_NewsFeed.Host.csproj", "src/Kakushkin_NewsFeed.Host/"]
COPY ["src/Kakushkin_NewsFeed.Abstractions/Kakushkin_NewsFeed.Abstractions.csproj", "src/Kakushkin_NewsFeed.Abstractions/"]
COPY ["src/Kakushkin_NewsFeed.Application/Kakushkin_NewsFeed.Application.csproj", "src/Kakushkin_NewsFeed.Application/"]
COPY ["src/Kakushkin_NewsFeed.Common/Kakushkin_NewsFeed.Common.csproj", "src/Kakushkin_NewsFeed.Common/"]
COPY ["src/Kakushkin_NewsFeed.Domain/Kakushkin_NewsFeed.Domain.csproj", "src/Kakushkin_NewsFeed.Domain/"]
COPY ["src//Kakushkin_NewsFeed.Persistence/Kakushkin_NewsFeed.Persistence.csproj", "src/Kakushkin_NewsFeed.Persistence/"]
RUN dotnet restore "src/Kakushkin_NewsFeed.Host/Kakushkin_NewsFeed.Host.csproj"
#RUN dotnet restore ./Kakushkin_NewsFeed.sln   #<ВАЖНО
COPY . .
WORKDIR "/src/src/Kakushkin_NewsFeed.Host"
RUN dotnet build "./Kakushkin_NewsFeed.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Kakushkin_NewsFeed.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kakushkin_NewsFeed.Host.dll"]
