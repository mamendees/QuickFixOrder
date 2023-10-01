FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /OrderAccumulator
EXPOSE 5001

COPY . .
RUN dotnet restore "OrderAccumulator.csproj"

RUN dotnet publish "OrderAccumulator.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /OrderAccumulator
COPY --from=build-env /OrderAccumulator/out .
ENTRYPOINT ["dotnet", "OrderAccumulator.dll"]