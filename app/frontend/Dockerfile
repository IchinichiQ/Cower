FROM node:20-alpine as build

# Specify where our app will live in the container
WORKDIR /app

# Copy the React App to the container
COPY . /app/

# Prepare the container for building React
RUN npm install
# We want the production version
RUN npm run build

# Prepare apache
FROM httpd:2.4
EXPOSE 443

RUN rm -rf /usr/local/apache2/htdocs
COPY --from=build /app/dist /usr/local/apache2/htdocs

RUN echo "LoadModule ssl_module modules/mod_ssl.so" | tee -a /usr/local/apache2/conf/httpd.conf
RUN echo "SSLEngine                on" | tee -a /usr/local/apache2/conf/httpd.conf
RUN echo "SSLCertificateFile       /etc/ssl/cower/certificate.crt" | tee -a /usr/local/apache2/conf/httpd.conf
RUN echo "SSLCertificateKeyFile    /etc/ssl/cower/private.key" | tee -a /usr/local/apache2/conf/httpd.conf
RUN echo "SSLCertificateChainFile  /etc/ssl/cower/ca_bundle.crt    " | tee -a /usr/local/apache2/conf/httpd.conf