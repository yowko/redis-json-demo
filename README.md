docker run -d --name redis-stack-server -p 6379:6379 -e REDIS_ARGS="--requirepass pass.123" redis/redis-stack-server:7.2.0-v11