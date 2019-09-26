FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
RUN apt-get update && apt-get dist-upgrade -y
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
RUN apt-get update && apt-get dist-upgrade -y
WORKDIR /src
COPY ["./src/IdentityServer.Testing/IdentityServer.Testing.csproj", "IdentityServer.Testing/"]
RUN dotnet restore "IdentityServer.Testing/IdentityServer.Testing.csproj"
COPY src .
WORKDIR "/src/IdentityServer.Testing"
RUN dotnet publish "IdentityServer.Testing.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "IdentityServer.Testing.dll"] 
