FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["src/OneReview/OneReview.csproj", "OneReview/"]
RUN dotnet restore 'OneReview/OneReview.csproj'

COPY ["src/OneReview", "OneReview/"]
WORKDIR /src/OneReview
RUN dotnet build 'OneReview.csproj' -c Release -o /app/build

FROM build AS publish
RUN dotnet publish 'OneReview.csproj' -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=5001
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "OneReview.dll" ]