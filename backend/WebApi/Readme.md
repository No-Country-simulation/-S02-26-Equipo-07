
# Construir la imagen
docker build -t webapi:latest .

# Ejecutar con entorno Development (Swagger visible)
docker run -d -p 5000:8080 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  --env-file .env \
  --name webapi-dev \
  webapi:latest