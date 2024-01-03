# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./PRN231-StudentManagmentSystem/StudentManagingSystem-API/StudentManagingSystem-API.csproj" --disable parallel
RUN dotnet publish "./PRN231-StudentManagmentSystem/StudentManagingSystem-API/StudentManagingSystem-API.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "StudentManagingSystem-API.dll"]