# 1. Build Phase
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["GeminiChat.API/GeminiChat.API.csproj", "GeminiChat.API/"]
RUN dotnet restore "GeminiChat.API/GeminiChat.API.csproj"

# Copy the rest of the source code
COPY . .

# Build and Publish the app
WORKDIR "/src/GeminiChat.API"
RUN dotnet publish "GeminiChat.API.csproj" -c Release -o /app/publish

# 2. Run Phase
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Configure for Render (Render uses port 8080 by default for Docker)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "GeminiChat.API.dll"]