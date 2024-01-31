#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /src
COPY ["StudentManagingSystem-API/StudentManagingSystem-API.csproj", "StudentManagingSystem-API/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["BusinessObject/BusinessObject.csproj", "BusinessObject/"]
RUN dotnet restore "StudentManagingSystem-API/StudentManagingSystem-API.csproj"
COPY . .
WORKDIR "/src/StudentManagingSystem-API"
RUN dotnet build "StudentManagingSystem-API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StudentManagingSystem-API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentManagingSystem-API.dll","--environment=Development"]