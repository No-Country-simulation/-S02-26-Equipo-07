# Guía de Despliegue a VPS.

## 🔄 Flujo de Despliegue Automático

```
1. Haces push a main (cambios en backend)
   ↓
2. GitHub Actions construye la imagen Docker
   ↓
3. Sube la imagen a Docker Hub
   ↓
4. Se conecta a tu VPS por SSH
   ↓
5. Descarga la última imagen de Docker Hub
   ↓
6. Para el contenedor anterior
   ↓
7. Inicia un nuevo contenedor con todas las credenciales
   ↓
8. ¡Aplicación desplegada automáticamente!
```

## 📋 Secretos de GitHub que debes configurar

### Ubicación: 
**Tu repositorio** → **Settings** → **Secrets and variables** → **Actions** → **New repository secret**

### 1. Credenciales de Docker Hub (2 secretos)
```
DOCKER_USERNAME = tu-usuario-de-dockerhub
DOCKER_TOKEN = tu-token-de-dockerhub
```
> Crea el token en: https://hub.docker.com/settings/security

### 2. Credenciales de tu VPS (4 secretos)
```
VPS_HOST = 192.168.1.100  (tu IP del VPS)
VPS_USERNAME = root  (o ubuntu, o tu usuario)
VPS_SSH_PRIVATE_KEY = (tu llave privada SSH completa)
VPS_PORT = 22  (o tu puerto SSH personalizado)
```

### 3. Variables de tu Aplicación (6 secretos)
```
JWT_SECRET_KEY = TuClaveSecretaMuySeguraMinimo32Caracteres!
JWT_ISSUER = WebApi
JWT_AUDIENCE = WebApiClient
JWT_EXPIRATION_HOURS = 24
DB_CONNECTION_STRING = Server=tu-servidor-db;Database=NC07WebApp;User Id=appuser;Password=TuPassword;TrustServerCertificate=True;MultipleActiveResultSets=true
CORS_ALLOWED_ORIGINS = https://tudominio.com,https://www.tudominio.com
```

## 🖥️ Requisitos en tu VPS

### 1. Instalar Docker
```bash
# Actualizar paquetes
sudo apt-get update

# Instalar Docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Permitir ejecutar Docker sin sudo
sudo usermod -aG docker $USER

# Verificar instalación
docker --version
```

### 2. Configurar SSH
Tu VPS debe permitir conexiones SSH con tu llave privada.

```bash
# Prueba la conexión desde tu máquina local
ssh -i /ruta/a/tu/llave-privada usuario@ip-vps
```

### 3. Abrir puertos necesarios
```bash
# Abrir puerto 80 (HTTP)
sudo ufw allow 80/tcp

# Abrir puerto 22 (SSH)
sudo ufw allow 22/tcp

# Activar firewall
sudo ufw enable

# Verificar reglas
sudo ufw status
```

## 🔍 Verificar tu VPS.

Sube este script a tu VPS y ejecútalo para verificar que todo está bien configurado:

```bash
# En tu VPS, ejecuta:
chmod +x vps-deployment-check.sh
./vps-deployment-check.sh
```

Este script verificará:
- ✓ Docker instalado y funcionando
- ✓ Puertos disponibles
- ✓ Acceso a Docker Hub
- ✓ Firewall configurado
- ✓ Espacio en disco



### Ver logs en tu VPS
```bash
# Conectarse al VPS
ssh usuario@ip-vps

# Ver si el contenedor está corriendo
docker ps | grep ecomerse_backend

# Ver logs del contenedor
docker logs ecomerse_backend

# Seguir logs en tiempo real
docker logs -f ecomerse_backend
```

## 🔧 Despliegue Manual (si es necesario)

Si necesitas desplegar manualmente:

```bash
# Conectarse al VPS
ssh usuario@ip-vps

# Login en Docker Hub
docker login -u tu-usuario

# Descargar última imagen
docker pull tu-usuario/ecomerse_backend:main

# Parar contenedor anterior
docker stop ecomerse_backend
docker rm ecomerse_backend

# Iniciar nuevo contenedor
docker run -d \
  --name ecomerse_backend \
  --restart unless-stopped \
  -p 80:80 \
  -e JWT_SECRET_KEY="tu-clave-secreta" \
  -e JWT_ISSUER="WebApi" \
  -e JWT_AUDIENCE="WebApiClient" \
  -e JWT_EXPIRATION_HOURS="24" \
  -e DB_CONNECTION_STRING="Server=...;Database=..." \
  -e CORS_ALLOWED_ORIGINS="https://tudominio.com" \
  -e ASPNETCORE_ENVIRONMENT="Production" \
  tu-usuario/ecomerse_backend:main
```


## Método Alternativo: Docker Compose

Si prefieres usar docker-compose en tu VPS:

```bash
# 1. Copiar docker-compose.prod.yml a tu VPS
scp docker-compose.prod.yml usuario@ip-vps:~/

# 2. Crear archivo .env en el VPS con tus variables
nano .env

# 3. Desplegar con docker-compose
docker-compose -f docker-compose.prod.yml up -d

# 4. Ver logs
docker-compose -f docker-compose.prod.yml logs -f
```
