# cli2api
cli2api is a generic wrapper around any command line interface (CLI) to expose it over a REST API endpoint.

## Simple example with ping command
Example with the basic ping command that will be exposed as a simple REST endpoint:
```
GET http://localhost/ping/google.com
```
Will display:
```
PING google.com (216.58.204.110) 56(84) bytes of data.
64 bytes from 216.58.204.110 (216.58.204.110): icmp_seq=1 ttl=37 time=30.8 ms

--- google.com ping statistics ---
1 packets transmitted, 1 received, 0% packet loss, time 0ms
rtt min/avg/max/mdev = 30.823/30.823/30.823/0.000 ms
```

## Generic synthax
More generally speaking the synthax of the rendered API looks like this:
```
GET http://localhost/{command}/{*arguments}
```

## Bundle your own CLI
Everything is based on Docker, we will use a Dockerfile.
Start with the CLI2API base docker image:
```
FROM laurentbel/cli2api
```
Then add whatever CLI (ping in our example) you want by installing a package or your own custom code:
```
RUN apt-get update && apt-get install -y iputils-ping
```

Now build your docker image and run it ! Note the environment variable cli2api_commands that lists the available commands:
```
docker build -t mycli2api-nslookup .
docker run -it --rm -p 80:80 --env cli2api_commands=nslookup mycli2api-nslookup
```

Open a web browser to http://localhost/nslookup/google.com it should display the following output:
```
Server:		192.168.65.5
Address:	192.168.65.5#53

Non-authoritative answer:
Name:	google.com
Address: 172.217.19.238
Name:	google.com
Address: 2a00:1450:4007:812::200e
```

## Protect your API with a key

Setting the environment variable cli2api_api_key will protect your api using a simple API Key.
You will be required to provide the key through basic http header named x-api-key otherwise you will receive a 401 HTTP error.
```
docker run -it --rm -p 80:80 --env cli2api_api_key=1234567890 --env cli2api_commands=nslookup laurentbel/cli2api-nslookup
```
In the above example, you will have to set the header "x-api-key" to the value 1234567890 if you want to be able to reach the endpoint http://localhost/nslookup/google.com  
Indeed you will need to change 1234567890 to something secret known by you.