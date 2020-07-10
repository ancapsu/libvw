# builder stage/image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

## install nodejs
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash -
RUN apt-get install -y nodejs

## copy csproj's
COPY ./Libraries/LibVisLib/LibVisLib/*.csproj ./Libraries/LibVisLib/LibVisLib/
COPY ./LibVisWeb/LibVisWeb/*.csproj ./LibVisWeb/LibVisWeb/

## restore LibVisWeb
RUN cd ./LibVisWeb/LibVisWeb/ && \
    dotnet restore

## restore LibVisLib
RUN cd ./Libraries/LibVisLib/LibVisLib/ && \
    dotnet restore

## copy full sources
COPY . .

## build LibVisWeb
WORKDIR /source/LibVisWeb/
RUN dotnet publish -c release -o /app --no-restore

## build LibVisLib
WORKDIR /source/Libraries/LibVisLib/
RUN dotnet publish -c release -o /app --no-restore


# final stage/image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /app
COPY --from=build /app .
EXPOSE 80
ENTRYPOINT ["dotnet", "LibVisWeb.dll", "--urls", "http://0.0.0.0;https://0.0.0.0"]
