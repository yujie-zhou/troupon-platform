#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["checkout-mock-service/checkout-mock-service.csproj", "checkout-mock-service/"]
RUN dotnet restore "checkout-mock-service/checkout-mock-service.csproj"
COPY . .
WORKDIR "/src/checkout-mock-service"
RUN dotnet build "checkout-mock-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "checkout-mock-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "checkout-mock-service.dll"]