#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#このDockerFile を右クリックで個別にビルドする場合
#Pao.Reports.Linux.dll がインストールフォルダに配置されていると
#Pathが遠すぎてビルドに失敗します。(.NET 5/6のバグと思われます。)
#従ってPao.Reports.Linux.dll等を近いパスにも配置して参照してあります。

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PaoWebApp/PaoWebApp.csproj", "PaoWebApp/"]
RUN dotnet restore "PaoWebApp/PaoWebApp.csproj"
COPY . .
WORKDIR "/src/PaoWebApp"
RUN dotnet build "PaoWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaoWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaoWebApp.dll"]

#このDockerFile を右クリックで個別にビルドしてDockerイメージを作成する場合
#帳票デザインファイルが必要なため、次の行が必要となります。
COPY ["PaoWebApp/App_Data/", "/app/App_Data/"]