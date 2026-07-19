FROM node:22-alpine AS build

WORKDIR /app

COPY package.json package-lock.json ./
RUN npm ci

COPY next.config.ts tsconfig.json ./
COPY public ./public
COPY src ./src

ARG NEXT_PUBLIC_JOURNEY_BFF_URL=http://localhost:8080
ENV NEXT_PUBLIC_JOURNEY_BFF_URL=${NEXT_PUBLIC_JOURNEY_BFF_URL}

RUN npm run build

FROM nginxinc/nginx-unprivileged:alpine AS runtime

COPY --from=build /app/out /usr/share/nginx/html

RUN sed -i 's/listen       8080;/listen       3000;/' /etc/nginx/conf.d/default.conf

EXPOSE 3000

CMD ["nginx", "-g", "daemon off;"]
