start /d ..\nginx\ nginx.exe

start "valuator1" /d ..\Valuator\ dotnet run --no-build --urls "http://localhost:5001"
start "valuator2" /d ..\Valuator\ dotnet run --no-build --urls "http://localhost:5002"

start "rankcalculator1" /d ..\RankCalculator\ dotnet run --no-build
start "rankcalculator2" /d ..\RankCalculator\ dotnet run --no-build

start "eventslogger1" /d ..\EventLogger\ dotnet run --no-build
start "eventslogger2" /d ..\EventLogger\ dotnet run --no-build