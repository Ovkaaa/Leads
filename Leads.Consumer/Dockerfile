# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["Leads.Consumer/Leads.Consumer.csproj", "Leads.Consumer/"]
COPY ["Leads.App/Leads.App.csproj", "Leads.App/"]
COPY ["Leads.Domain/Leads.Domain.csproj", "Leads.Domain/"]
COPY ["Leads.Infrastructure/Leads.Infrastructure.csproj", "Leads.Infrastructure/"]
RUN dotnet restore "Leads.Consumer/Leads.Consumer.csproj"

# Copy the remaining files and build the app
COPY . .
WORKDIR "/src/Leads.Consumer"
RUN dotnet build "Leads.Consumer.csproj" -c Release -o /app/build
RUN dotnet publish "Leads.Consumer.csproj" -c Release -o /app/publish

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Leads.Consumer.dll"]
