FROM microsoft/aspnetcore:2.0
FROM microsoft/dotnet:2.0.0-sdk-2.0.2-stretch

ENV CORECLR_ENABLE_PROFILING="1" \
CORECLR_PROFILER="{36032161-FFC0-4B61-B559-F6C5D41BAE5A}" \
CORECLR_NEWRELIC_HOME="/usr/local/newrelic-netcore20-agent" \
CORECLR_PROFILER_PATH="/usr/local/newrelic-netcore20-agent/libNewRelicProfiler.so" \
NEW_RELIC_LICENSE_KEY="3746113fec7f21e85135087aa6d47225369db05e"


ARG source
WORKDIR /app


ARG NewRelic=./newrelic
COPY $NewRelic ./newrelic

RUN dpkg -i ./newrelic/newrelic-netcore20-agent*.deb

ENV ASPNETCORE_URLS http://+:8601
EXPOSE 8601
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Core.Services.dll"]
