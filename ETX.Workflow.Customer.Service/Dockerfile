#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ETX.Workflow.Customer.Service/ETX.Workflow.Customer.Service.csproj", "ETX.Workflow.Customer.Service/"]
COPY ["ETX.Workflow.Customer.Infrastructure/ETX.Workflow.Customer.Infrastructure.csproj", "ETX.Workflow.Customer.Infrastructure/"]
COPY ["ETX.Workflow.Customer.Application/ETX.Workflow.Customer.Application.csproj", "ETX.Workflow.Customer.Application/"]
COPY ["ETX.Workflow.Customer.Domain/ETX.Workflow.Customer.Domain.csproj", "ETX.Workflow.Customer.Domain/"]
COPY ["ETX.Workflow.Customer.Persistence/ETX.Workflow.Customer.Persistence.csproj", "ETX.Workflow.Customer.Persistence/"]
RUN dotnet restore "ETX.Workflow.Customer.Service/ETX.Workflow.Customer.Service.csproj"
COPY . .
WORKDIR "/src/ETX.Workflow.Customer.Service"
RUN dotnet build "ETX.Workflow.Customer.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ETX.Workflow.Customer.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ETX.Workflow.Customer.Service.dll"]