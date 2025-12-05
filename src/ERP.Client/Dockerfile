# ETAPA 1: Construcción (Node.js)
FROM node:20-alpine as build-stage
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
# Generar los archivos estáticos en /dist
RUN npm run build

# ETAPA 2: Servidor Web (Nginx)
FROM nginx:stable-alpine as production-stage
# Copiar los archivos generados al servidor Nginx
COPY --from=build-stage /app/dist /usr/share/nginx/html
# Configuración para que el router de Vue funcione (SPA)
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]