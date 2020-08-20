FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS image

FROM image as stage
WORKDIR /
COPY [".", "."]
RUN dotnet restore

FROM stage AS build
RUN dotnet build --no-restore -c Release

FROM build AS publish
RUN dotnet publish --no-build -c Release -o /app/publish /src/ChildcareWorldwide.Integration.Manager/ChildcareWorldwide.Integration.Manager.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "ChildcareWorldwide.Integration.Manager.dll"]
