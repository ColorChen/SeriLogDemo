#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
# setup exec environment
# ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_ENVIRONMENT=Staging
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SeriLogDemo_Net6.csproj", ""]
RUN dotnet restore "./SeriLogDemo_Net6.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SeriLogDemo_Net6.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SeriLogDemo_Net6.csproj" -c Release -o /app/publish



FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# 安裝網路工具
RUN apt update
RUN apt install net-tools

ENTRYPOINT ["dotnet", "SeriLogDemo_Net6.dll"]