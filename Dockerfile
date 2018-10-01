FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Api.Customers/*.csproj ./Api.Customers/
COPY Api.CustomersUnitTests/*.csproj ./Api.CustomersUnitTests/
RUN dotnet restore

# copy everything else and build app
COPY . .
WORKDIR /app/Api.Customers
RUN dotnet build

# setup tests
FROM build AS testrunner
WORKDIR /app/Api.CustomersUnitTests
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

FROM build AS test
WORKDIR /app/Api.CustomersUnitTests
RUN dotnet test

# build
FROM build AS publish
WORKDIR /app/Api.Customers
RUN dotnet publish -c Release -o out

# build runtime image
FROM microsoft/dotnet:2.1.3-aspnetcore-runtime-alpine3.7 AS runtime
WORKDIR /app
COPY --from=publish /app/Api.Customers/out ./
ENTRYPOINT ["dotnet", "Api.Customers.dll"]

