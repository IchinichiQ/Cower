FROM node:20-alpine as build

# Specify where our app will live in the container
WORKDIR /app

# Copy the React App to the container
COPY . /app/

# Prepare the container for building React
RUN npm install

ARG BACKEND_ADDRESS
ENV VITE_BACKEND_ADDRESS=$BACKEND_ADDRESS

# We want the production version
RUN npm run build

# Prepare apache
FROM httpd:2.4

# For debug
ARG BACKEND_ADDRESS
ENV VITE_BACKEND_ADDRESS=$BACKEND_ADDRESS

# Move built project
RUN rm -rf /usr/local/apache2/htdocs
COPY --from=build /app/dist /usr/local/apache2/htdocs

# Use ssl
RUN if [ -f /cower/ssl/certificate.crt ]; then \
        echo "LoadModule               ssl_module modules/mod_ssl.so" >> /usr/local/apache2/conf/httpd.conf; \
        echo "SSLEngine                on" >> /usr/local/apache2/conf/httpd.conf; \
        echo "SSLCertificateFile       /cower/ssl/certificate.crt" >> /usr/local/apache2/conf/httpd.conf; \
        echo "SSLCertificateKeyFile    /cower/ssl/private.key" >> /usr/local/apache2/conf/httpd.conf; \
        echo "SSLCertificateChainFile  /cower/ssl/ca_bundle.crt" >> /usr/local/apache2/conf/httpd.conf; \
    fi

RUN { \
    echo '#!/bin/sh';  \
    echo 'grep -rl https://replace_me_in_container /usr/local/apache2/htdocs/ | xargs sed -i "s|https://replace_me_in_container|$BACKEND_ADDRESS|g"'; \
    echo 'httpd-foreground'; \
} > /usr/local/apache2/start
RUN chmod +x /usr/local/apache2/start

ENTRYPOINT ["/usr/local/apache2/start"]