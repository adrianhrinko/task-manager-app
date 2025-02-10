# Development
FROM node:18 AS debug
WORKDIR /app
EXPOSE 5173 
EXPOSE 9229
COPY package.json package-lock.json ./
RUN npm install
CMD ["npm", "run", "dev", "--", "--host"]

# Production
FROM node:18 AS build
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build

FROM nginx:1.21 AS release
EXPOSE 5173
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]