FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY out/ /app
ENTRYPOINT [ "dotnet", "/app/helloApi.dll --server.urls http://47.100.193.29:5021" ]
