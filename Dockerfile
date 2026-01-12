# 1. Build Phase - UPDATED TO 9.0
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file
COPY ["GeminiChat.API/GeminiChat.API.csproj", "GeminiChat.API/"]
RUN dotnet restore "GeminiChat.API/GeminiChat.API.csproj"

# Copy everything else
COPY . .

# Build and Publish
WORKDIR "/src/GeminiChat.API"
RUN dotnet publish "GeminiChat.API.csproj" -c Release -o /app/publish

# 2. Run Phase - UPDATED TO 9.0
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render uses port 8080 by default
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "GeminiChat.API.dll"]