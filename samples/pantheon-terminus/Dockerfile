FROM laurentbel/cli2api

RUN apt-get update && \
    apt-get -y install curl git php php-common php-curl php-xml

WORKDIR /cli
RUN curl -O https://raw.githubusercontent.com/pantheon-systems/terminus-installer/master/builds/installer.phar && php installer.phar install