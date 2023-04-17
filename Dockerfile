FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
COPY Src/WorkingGood.Domain/WorkingGood.Domain.csproj ./Domain/
COPY Src/WorkingGood.Infrastructure/WorkingGood.Infrastructure.csproj ./Infrastructure/
COPY Src/WorkingGood.WebApi/WorkingGood.WebApi.csproj ./WebApi/
RUN dotnet restore ./WebApi/WorkingGood.WebApi.csproj
COPY . ./
RUN dotnet publish ./WorkingGood.Applications.sln -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "WorkingGood.WebApi.dll"]

ENV ASPNETCORE_ENVIRONMENT="Development"
ENV TZ="Europe/Warsaw"
EXPOSE 80
EXPOSE 443