FROM cli2api

# install Python 3.7.4
RUN apt-get update &&  apt-get install -y wget build-essential checkinstall libncursesw5-dev libssl-dev libsqlite3-dev tk-dev libgdbm-dev libc6-dev libbz2-dev libffi-dev zlib1g-dev git
RUN wget https://www.python.org/ftp/python/3.7.4/Python-3.7.4.tgz
RUN tar xzf Python-3.7.4.tgz
WORKDIR /Python-3.7.4
RUN ./configure --enable-optimizations
RUN make install

# install the DrHeader app from https://github.com/Santandersecurityresearch/DrHeader
WORKDIR /cli
RUN git clone --depth=1 https://github.com/Santandersecurityresearch/DrHeader.git
WORKDIR /cli/DrHeader
RUN pip3 install .
RUN python3 setup.py install --user