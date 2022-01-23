# cli2api - ping

ping command over API

## Build
```
docker build -t cli2api-ping .
```

## Run
```
docker run -it --rm -p 80:80 --env cli2api_commands=ping cli2api-ping
```
Then browse http://localhost/ping/-c/3/google.com