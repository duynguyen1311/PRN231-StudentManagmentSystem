#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /src
COPY ["StudentManagingSystem-Client/StudentManagingSystem-Client.csproj", "StudentManagingSystem-Client/"]
COPY ["BusinessObject/BusinessObject.csproj", "BusinessObject/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
RUN dotnet restore "StudentManagingSystem-Client/StudentManagingSystem-Client.csproj"
COPY . .
WORKDIR "/src/StudentManagingSystem-Client"
RUN dotnet build "StudentManagingSystem-Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StudentManagingSystem-Client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentManagingSystem-Client.dll","--environment=Development"]