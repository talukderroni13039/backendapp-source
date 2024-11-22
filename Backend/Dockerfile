FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
ENV ASPNETCORE_URLS=http://+:8080
WORKDIR /app
# Ensure we listen on any IP Address 

# Copy everything
COPY . ./

# Restore and publish for MsgProcessor project
WORKDIR /app/Backend.Api
RUN dotnet restore ./Backend.Api.csproj
RUN dotnet publish -c Release -o out   
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build-env /app/Backend.Api/out .

ENTRYPOINT ["dotnet", "Backend.Api.dll"]
