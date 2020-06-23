FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY out/recommendapi /app
ENTRYPOINT [ "dotnet", "/app/Recommend.API.dll"]
