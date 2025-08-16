# コマンドライン使用時(例): docker build -t serverlinux-webapi .
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#このDockerFile を右クリックで個別にビルドする場合
#Pao.Reports.Linux.dll がインストールフォルダに配置されていると
#Pathが遠すぎてビルドに失敗します。(.NET 5～9以降のバグと思われます。)
#従ってPao.Reports.Linux.dll等を近いパスにも配置して参照してあります。


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Server/Server.Linux.csproj", "Server/"]
RUN dotnet restore "Server/Server.Linux.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "Server.Linux.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Server.Linux.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Server.Linux.dll"]

#このDockerFile を右クリックで個別にビルドしてDockerイメージを作成する場合
#帳票デザインファイルが必要なため、次の行が必要となります。
COPY ["Server/App_Data/", "/app/App_Data/"]