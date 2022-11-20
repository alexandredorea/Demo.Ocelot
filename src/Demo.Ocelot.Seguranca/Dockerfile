#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Demo.Ocelot.Seguranca/Demo.Ocelot.Seguranca.csproj", "Demo.Ocelot.Seguranca/"]
RUN dotnet restore "Demo.Ocelot.Seguranca/Demo.Ocelot.Seguranca.csproj"
COPY . .
WORKDIR "/src/Demo.Ocelot.Seguranca"
RUN dotnet build "Demo.Ocelot.Seguranca.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Demo.Ocelot.Seguranca.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.Ocelot.Seguranca.dll"]