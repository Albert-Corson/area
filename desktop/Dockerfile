FROM node:fermium-alpine

WORKDIR /app

EXPOSE 8080

RUN npm install
RUN npm rebuild node-sass
CMD npm run serve
