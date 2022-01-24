# cli2api - DrHeader

DrHeader Python script command over API.  
Based on https://github.com/Santandersecurityresearch/DrHeader

## Build
```
docker build -t cli2api-dr-header .
```

## Run
```
docker run -it --rm -p 80:80 --env cli2api_commands=drheader --env cli2api_suffix_argument="--json" cli2api-dr-header
```
Then browse http://localhost/drheader/scan/single/https%3A%2F%2Fgoogle.com