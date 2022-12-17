# add sdk NET 5.0 to the image (stage 1)
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy th project file
COPY ["CarseerWebAPP.csproj", "."]
# restore all dependancies
RUN dotnet restore "./CarseerWebAPP.csproj"

# copy & deploy the application files 

COPY . .
RUN dotnet publish -c release -o /app

# add asp NET Runtime 5.0 to the image (stage 2)
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR  /app
COPY --from=build /app .

ENTRYPOINT [ "dotnet", "CarseerWebAPP.dll" ]
