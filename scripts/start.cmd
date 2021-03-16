docker run --name redis -p 6379:6379 -d redis
docker run --name nats -p 4222:4222 -p 6222:6222 -p 8222:8222 -d nats

start /d ..\Valuator\ dotnet run --no-build --urls "http://localhost:5001"
start /d ..\Valuator\ dotnet run --no-build --urls "http://localhost:5002"

start /d ..\nginx\ nginx.exe

start /d ..\RankCalculator\ dotnet run --no-build
start /d ..\RankCalculator\ dotnet run --no-build