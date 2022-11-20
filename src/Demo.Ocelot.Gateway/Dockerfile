#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Demo.Ocelot.Gateway/Demo.Ocelot.Gateway.csproj", "src/Demo.Ocelot.Gateway/"]
RUN dotnet restore "src/Demo.Ocelot.Gateway/Demo.Ocelot.Gateway.csproj"
COPY . .
WORKDIR "/src/src/Demo.Ocelot.Gateway"
RUN dotnet build "Demo.Ocelot.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Demo.Ocelot.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.Ocelot.Gateway.dll"]