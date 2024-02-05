# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy only the necessary files for restoring and building
COPY TrackingSystem/PixelService/PixelService.csproj /app/PixelService/
COPY TrackingSystem/Shared/Shared.csproj /app/Shared/
RUN dotnet restore /app/Shared/
RUN dotnet restore /app/PixelService/

# Copy the rest of the application files
COPY . .

RUN dotnet build "TrackingSystem/PixelService/PixelService.csproj" -c Release -o /app/build

# Stage 2: Publish Stage
FROM build AS publish
RUN dotnet publish "TrackingSystem/PixelService/PixelService.csproj" -c Release -o /app/publish /p:UseAppHost=false


# Stage 3: Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy only the necessary files from the build stage
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:5272

# Expose the port the application will run on
EXPOSE 5272

# Define the entry point for the runtime image
ENTRYPOINT ["dotnet", "PixelService.dll"]
