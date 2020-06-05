FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY out/useridentity /app
ENTRYPOINT [ "dotnet", "/app/User.Identity.dll"]
