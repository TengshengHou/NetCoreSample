FROM mcr.microsoft.com/dotnet/core/sdk
WORKDIR /app
COPY User.Api/out /app
ENTRYPOINT [ "dotnet", "/app/User.Api.dll" ]