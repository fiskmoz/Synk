FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0
COPY Synk.csproj /build/
RUN dotnet restore ./build/Synk.csproj
COPY . ./build/
WORKDIR /build/
RUN dotnet publish ./Synk.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /build/out ./
ENTRYPOINT ["dotnet", "Synk.dll"]