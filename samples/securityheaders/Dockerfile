FROM laurentbel/cli2api

# install python 2 
RUN apt-get update && apt-get install -y python2 git

# install pip
RUN apt install -y curl
RUN curl https://bootstrap.pypa.io/pip/2.7/get-pip.py --output get-pip.py
RUN python2 get-pip.py

# install the securityheaders app from https://github.com/koenbuyens/securityheaders/
WORKDIR /cli
RUN git clone --depth=1 https://github.com/koenbuyens/securityheaders.git
WORKDIR /cli/securityheaders
RUN pip2 install --no-cache-dir -r requirements.txt