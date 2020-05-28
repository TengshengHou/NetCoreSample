FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY out/gateway /app
ENTRYPOINT [ "dotnet", "/app/Gateway.API.dll"]
