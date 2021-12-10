# cli2api - pantheon-terminus

Pantheon terminus CLI over API

## Build
```
docker build -t cli2api-pantheon-terminus .
```

## Run
```
docker run -it --rm -p 80:80 --env cli2api:commands=terminus cli2api-pantheon-terminus
```
Then browse http://localhost/terminus