# cli2api - securityheaders

Securityheader Python script command over API.  
Based on https://github.com/koenbuyens/securityheaders

## Build
```
docker build -t cli2api-securityheaders .
```

## Run
```
docker run -it --rm -p 80:80 --env cli2api_commands=python2 --env cli2api_suffix_argument="--formatter json" cli2api-securityheaders
```
Then browse http://localhost/python2/securityheaders.py/google.com