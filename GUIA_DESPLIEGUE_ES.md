# Guía de Despliegue Automático a VPS - Resumen en Español

## 🎯 ¿Qué se ha implementado?

Se ha configurado el CI/CD para que automáticamente después de construir la imagen Docker y subirla a Docker Hub, se despliegue en tu VPS.

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

## 📁 Archivos Creados

1. **`.github/workflows/docker-build.yml`** - Modificado para incluir despliegue a VPS
2. **`DEPLOYMENT_SETUP.md`** - Guía completa en inglés
3. **`GITHUB_SECRETS_GUIDE.md`** - Referencia rápida de secretos
4. **`vps-deployment-check.sh`** - Script para verificar tu VPS
5. **`docker-compose.prod.yml`** - Método alternativo con docker-compose
6. **`.env.production.example`** - Ejemplo de variables de entorno
7. **`GUIA_DESPLIEGUE_ES.md`** - Esta guía en español

## 🔍 Verificar tu VPS

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

## 🚀 Cómo usar

### Después de configurar todos los secretos:

1. Haz un cambio en el código del backend
2. Haz commit y push a la rama `main`
3. Ve a la pestaña **Actions** en GitHub
4. Observa cómo se ejecuta el despliegue automático
5. Cuando termine, tu aplicación estará en `http://tu-ip-vps`

## 📊 Monitorear Despliegues

### Ver logs en GitHub
1. Ve a tu repositorio en GitHub
2. Click en pestaña **Actions**
3. Selecciona el workflow más reciente
4. Click en el job `build-and-push`
5. Expande el paso `Deploy to VPS` para ver los logs

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

## ❓ Solución de Problemas Comunes

### Error: No se puede conectar por SSH
- Verifica que `VPS_HOST` sea la IP correcta
- Verifica que `VPS_PORT` sea correcto (usualmente 22)
- Asegúrate de que la llave privada esté completa (con `-----BEGIN...` y `-----END...`)
- Verifica que el VPS permita conexiones SSH

### Error: No se puede descargar la imagen
- Verifica `DOCKER_USERNAME` y `DOCKER_TOKEN`
- Verifica que el token tenga permisos de lectura
- Asegúrate de que el nombre de la imagen sea correcto

### Error: El contenedor no inicia
```bash
# Ver los logs del contenedor
docker logs ecomerse_backend
```
Posibles causas:
- Cadena de conexión a la base de datos incorrecta
- Base de datos no accesible desde el VPS
- Variables de entorno faltantes

### Error: No se puede conectar a la base de datos
- Verifica que la base de datos permita conexiones desde la IP del VPS
- Verifica la cadena de conexión
- Asegúrate de que el puerto de la base de datos (1433) esté abierto

```bash
# Probar conectividad desde el VPS
telnet servidor-db 1433
# o
nc -zv servidor-db 1433
```

## 🔐 Formato de la Llave SSH Privada

Tu llave privada SSH debe incluir todo, incluyendo los encabezados:

```
-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAABlwAAAAdzc2gtcn
... (muchas líneas) ...
-----END OPENSSH PRIVATE KEY-----
```

**Importante:**
- Incluye las líneas `-----BEGIN` y `-----END`
- Copia toda la llave con los saltos de línea
- No agregues espacios o formato extra

### Cómo obtener tu llave SSH

```bash
# En tu máquina local, genera un nuevo par de llaves
ssh-keygen -t rsa -b 4096 -C "tu_email@example.com"

# Esto crea:
# - Llave privada: ~/.ssh/id_rsa (¡mantén esto en secreto!)
# - Llave pública: ~/.ssh/id_rsa.pub (copia esto al VPS)

# Copiar llave pública al VPS
ssh-copy-id usuario@ip-vps

# Ver contenido de llave privada (para GitHub secret)
cat ~/.ssh/id_rsa
# Copia TODO el contenido
```

## ✅ Lista de Verificación

Antes de hacer push:

- [ ] Todos los secretos de GitHub configurados
- [ ] Docker instalado en VPS
- [ ] Puertos 22 y 80 abiertos en VPS
- [ ] Conexión SSH funcionando
- [ ] Base de datos accesible desde VPS
- [ ] Cadena de conexión correcta

## 📞 Soporte

Si tienes problemas:

1. Revisa los logs de GitHub Actions
2. Revisa los logs del contenedor en el VPS: `docker logs ecomerse_backend`
3. Ejecuta el script de verificación: `./vps-deployment-check.sh`
4. Verifica que todos los secretos estén configurados correctamente

## 🎉 Próximos Pasos

Después de un despliegue exitoso:

1. ✓ Probar los endpoints de la API
2. ✓ Configurar HTTPS (recomendado para producción)
3. ✓ Configurar monitoreo
4. ✓ Configurar respaldos de base de datos
5. ✓ Configurar nombre de dominio
6. ✓ Agregar endpoint de health check

## 💡 Método Alternativo: Docker Compose

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

---

## 🎯 Resumen

Has implementado un sistema de CI/CD completo que:
- ✅ Construye automáticamente tu imagen Docker
- ✅ La sube a Docker Hub
- ✅ Se conecta a tu VPS
- ✅ Descarga y despliega la última versión
- ✅ Configura todas las variables de entorno necesarias
- ✅ Reinicia el servicio automáticamente

¡Solo necesitas configurar los secretos y todo funcionará automáticamente!
