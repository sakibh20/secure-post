# Stage 1: Build the application using .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
 
# Copy project file and restore dependencies
COPY ["SecureService.API/SecureService.API.csproj", "SecureService.API/"]
COPY ["SecureService.BLL/SecureService.BLL.csproj", "SecureService.BLL/"]
COPY ["SecureService.DAL/SecureService.DAL.csproj", "SecureService.DAL/"]
COPY ["SecureService.Context/SecureService.Context.csproj", "SecureService.Context/"]
COPY ["SecureService.Entity/SecureService.Entity.csproj", "SecureService.Entity/"]
COPY ["SecureService.Logging/SecureService.Logging.csproj", "SecureService.Logging/"]
RUN dotnet restore "./SecureService.API/SecureService.API.csproj"
 
# Copy everything and build the application
COPY . .
RUN dotnet publish -c Release -o /app/publish
 
# Stage 2: Create a runtime image using .NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
 
# Copy the published application from the build stage
COPY --from=build /app/publish .
 
# Expose ports for the application
EXPOSE 8080
 
# Set the entry point to run the application
CMD ["dotnet", "SecureService.API.dll"]