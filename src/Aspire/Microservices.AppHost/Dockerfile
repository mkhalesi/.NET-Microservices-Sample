# Ensure the Service Fabric cluster you deploy to supports the container image you are using. See https://aka.ms/containerimagehelp for information on Windows container version compatibility.
FROM mcr.microsoft.com/dotnet/runtime:8.0-nanoserver-1809
ARG source
WORKDIR /app
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Microservices.AppHost.dll"]
