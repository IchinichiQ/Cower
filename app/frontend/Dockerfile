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

# Move built project
RUN rm -rf /usr/local/apache2/htdocs
COPY --from=build /app/dist /usr/local/apache2/htdocs

# Use ssl
RUN { \
    echo "LoadModule               ssl_module modules/mod_ssl.so"; \
    echo "SSLEngine                on"; \
    echo "SSLCertificateFile       /etc/ssl/cower/certificate.crt"; \
    echo "SSLCertificateKeyFile    /etc/ssl/cower/private.key"; \
    echo "SSLCertificateChainFile  /etc/ssl/cower/ca_bundle.crt"; \
} | tee -a /usr/local/apache2/conf/httpd.conf