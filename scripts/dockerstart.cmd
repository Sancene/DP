docker run --name Redis -p 6379:6379 -d redis
docker run --name RedisRU -p 6000:6379 -d redis
docker run --name RedisEU -p 6001:6379 -d redis
docker run --name RedisOther -p 6002:6379 -d redis
docker run --name Nats -p 4222:4222 -p 6222:6222 -p 8222:8222 -d nats