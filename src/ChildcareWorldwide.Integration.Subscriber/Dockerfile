FROM mcr.microsoft.com/dotnet/sdk:5.0 AS image

FROM image as stage
WORKDIR /
COPY [".", "."]
RUN dotnet restore

FROM stage AS build
RUN dotnet build --no-restore -c Release

FROM build AS publish
RUN dotnet publish --no-build -c Release -o /app/publish /src/ChildcareWorldwide.Integration.Subscriber/ChildcareWorldwide.Integration.Subscriber.csproj

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
# workaround for https://github.com/grpc/grpc/issues/24153
RUN apt-get update && apt-get install -y libc-dev && apt-get clean

WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "ChildcareWorldwide.Integration.Subscriber.dll"]
