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

Now build your docker image and run it ! Note the environment variable cli2api:commands that lists the available commands:
```
docker build -t mycli2api-nslookup .
docker run -it --rm -p 80:80 --env cli2api:commands=nslookup cli2api-nslookup
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

