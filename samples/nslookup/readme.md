# cli2api - nslookup

nslookup command over API.

## Build
```
docker build -t cli2api-nslookup .
```

## Run
```
docker run -it --rm -p 80:80 --env cli2api:commands=nslookup cli2api-nslookup
```
Then browse http://localhost/nslookup/google.com