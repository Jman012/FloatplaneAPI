FROM node:16 as build
RUN apt-get update && apt-get -y install openjdk-11-jre-headless


COPY ./package.json ./package-lock.json* /
WORKDIR /
RUN npm ci && npm cache clean --force

COPY ./ /
RUN make docs-all

#Copy static files to Nginx
FROM nginx:alpine
COPY --from=build /Docs /usr/share/nginx/html

WORKDIR /usr/share/nginx/html
