# Diagrama de Flujo del Despliegue Automático

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         FLUJO DE DESPLIEGUE AUTOMÁTICO                      │
└─────────────────────────────────────────────────────────────────────────────┘

    Developer                GitHub Actions              Docker Hub           VPS
        │                           │                         │               │
        │  1. git push main         │                         │               │
        │──────────────────────────>│                         │               │
        │                           │                         │               │
        │                           │ 2. Checkout code        │               │
        │                           │                         │               │
        │                           │ 3. Login to Docker Hub  │               │
        │                           │────────────────────────>│               │
        │                           │                         │               │
        │                           │ 4. Build Docker image   │               │
        │                           │    (backend/WebApi)     │               │
        │                           │                         │               │
        │                           │ 5. Push image           │               │
        │                           │────────────────────────>│               │
        │                           │                         │               │
        │                           │ 6. SSH Connect          │               │
        │                           │────────────────────────────────────────>│
        │                           │                         │               │
        │                           │                         │ 7. Login      │
        │                           │                         │<──────────────│
        │                           │                         │               │
        │                           │                         │ 8. Pull image │
        │                           │                         │<──────────────│
        │                           │                         │               │
        │                           │                         │ 9. Stop old   │
        │                           │                         │    container  │
        │                           │                         │               │
        │                           │                         │ 10. Start new │
        │                           │                         │     container │
        │                           │                         │     with envs │
        │                           │                         │               │
        │                           │ 11. Deployment Success  │               │
        │                           │<────────────────────────────────────────│
        │                           │                         │               │
        │  12. Notification         │                         │               │
        │<──────────────────────────│                         │               │
        │                           │                         │               │


┌─────────────────────────────────────────────────────────────────────────────┐
│                           COMPONENTES DEL SISTEMA                           │
└─────────────────────────────────────────────────────────────────────────────┘

┌──────────────────────┐
│   GitHub Repository  │
│                      │
│  .github/workflows/  │
│  └── docker-build.yml│  ← Pipeline de CI/CD modificado
└──────────────────────┘

┌──────────────────────┐
│   GitHub Secrets     │
│                      │
│  • DOCKER_USERNAME   │
│  • DOCKER_TOKEN      │
│  • VPS_HOST          │
│  • VPS_USERNAME      │
│  • VPS_SSH_PRIVATE   │
│  • VPS_PORT          │
│  • JWT_SECRET_KEY    │
│  • DB_CONNECTION_STR │
│  • CORS_ORIGINS      │
│  • etc...            │
└──────────────────────┘

┌──────────────────────┐
│    Docker Hub        │
│                      │
│  username/           │
│  ecomerse_backend:   │
│    - main            │
│    - latest          │
└──────────────────────┘

┌──────────────────────┐
│       VPS Server     │
│                      │
│  Docker Daemon       │
│  └── Container:      │
│      ecomerse_backend│
│                      │
│  Port 80 → Internet  │
└──────────────────────┘

┌──────────────────────┐
│  Database Server     │
│  (External)          │
│                      │
│  SQL Server          │
│  NC07WebApp DB       │
└──────────────────────┘


┌─────────────────────────────────────────────────────────────────────────────┐
│                      CONFIGURACIÓN DEL CONTENEDOR                           │
└─────────────────────────────────────────────────────────────────────────────┘

Container: ecomerse_backend
├── Image: username/ecomerse_backend:main
├── Restart Policy: unless-stopped
├── Port Mapping: 80:80
├── Environment Variables:
│   ├── JWT_SECRET_KEY
│   ├── JWT_ISSUER
│   ├── JWT_AUDIENCE
│   ├── JWT_EXPIRATION_HOURS
│   ├── DB_CONNECTION_STRING
│   ├── CORS_ALLOWED_ORIGINS
│   └── ASPNETCORE_ENVIRONMENT=Production
└── Network: Bridge (default)


┌─────────────────────────────────────────────────────────────────────────────┐
│                         ARCHIVOS DE DOCUMENTACIÓN                           │
└─────────────────────────────────────────────────────────────────────────────┘

📄 GUIA_DESPLIEGUE_ES.md
   └── Guía completa en español con todos los pasos

📄 DEPLOYMENT_SETUP.md
   └── Guía técnica detallada en inglés

📄 GITHUB_SECRETS_GUIDE.md
   └── Lista de todos los secretos necesarios

📄 docker-compose.prod.yml
   └── Archivo alternativo para despliegue con docker-compose

📄 .env.production.example
   └── Ejemplo de variables de entorno

🔧 vps-deployment-check.sh
   └── Script para verificar configuración del VPS

📝 README.md
   └── README actualizado con enlaces a documentación


┌─────────────────────────────────────────────────────────────────────────────┐
│                           SEGURIDAD Y MEJORES PRÁCTICAS                     │
└─────────────────────────────────────────────────────────────────────────────┘

✓ Todas las credenciales en GitHub Secrets (nunca en código)
✓ Conexión SSH con llave privada (sin contraseñas)
✓ Variables de entorno pasadas al contenedor
✓ Restart policy para alta disponibilidad
✓ Limpieza automática de imágenes antiguas
✓ Logs disponibles para debugging
✓ Base de datos en servidor separado
✓ CORS configurado para dominios específicos


┌─────────────────────────────────────────────────────────────────────────────┐
│                              PRÓXIMOS PASOS                                 │
└─────────────────────────────────────────────────────────────────────────────┘

1. Configurar los 12 GitHub Secrets requeridos
2. Asegurarse de que el VPS tenga Docker instalado
3. Verificar conectividad SSH al VPS
4. Abrir puertos 22 y 80 en el firewall del VPS
5. Hacer push a main para probar el despliegue
6. Monitorear en GitHub Actions → pestaña Actions
7. Verificar que la aplicación esté corriendo en http://VPS_IP
8. (Opcional) Configurar HTTPS con Let's Encrypt
9. (Opcional) Configurar dominio personalizado
10. (Opcional) Configurar monitoreo y alertas
```
