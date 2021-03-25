docker stop redis & docker rm redis
docker stop nats & docker rm nats

taskkill /f /im valuator.exe
taskkill /f /im rankcalculator.exe
taskkill /f /im eventlogger.exe

taskkill /f /im nginx.exe