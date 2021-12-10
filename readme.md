# CLI2API
cli2api is a generic wrapper around any command line interface (CLI) to expose it over a REST API endpoint.

## Simple example with PING command:
Let take the example of the ping command:
```
ping google.com
```
It will be exposed over a simple REST endpoint:
```
GET http://localhost/cli2api/google.com
```

## Generic synthax
More generally speaking the synthax of the rendered API looks like this:
```
GET http://localhost/{command}/{*arguments}
```

## Build your own
Everything is based on Docker.  
Start with the CLI2API base docker image:
```
FROM pernodricard/cli2api
```
Then add whatever CLI (ping in our example) you want by installing a package or your own custom code:
```
RUN apt-get update && apt-get install -y iputils-ping
```
Make a permanent alias, named cli2api, to the command you want to run:
```
RUN command='ping -c 3'
RUN echo "alias cli2api='$command'" >> ~/.bashrc
```
In the above example, simply replace 'ping -c 3' by any command you like. 

That's it, we are done !

Simply build your docker image and run it !
```
docker build -t mycli2api .
docker run -it --rm -p 80:80 mycli2api
```

Open a web browser and browse to http://localhost/google.com it should display the following output:
```

