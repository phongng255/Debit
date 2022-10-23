#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Debit/Debit.csproj", "Debit/"]
RUN dotnet restore "Debit/Debit.csproj"
COPY . .
WORKDIR "/src/Debit"
RUN dotnet build "Debit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Debit.csproj" -c Release -o /app/publish

RUN apt update && apt install tzdata -y
ENV TZ="Asia/Ho_Chi_Minh"

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Debit.dll"]