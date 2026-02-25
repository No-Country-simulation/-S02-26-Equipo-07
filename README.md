# -S02-26-Equipo-07
Monorepo para equipo S02-26-Equipo 07-Web App Development de no country

## 🚀 Automatic VPS Deployment

This project includes automatic deployment to VPS after building Docker images.

### Quick Setup

1. **Configure GitHub Secrets** - See [GITHUB_SECRETS_GUIDE.md](GITHUB_SECRETS_GUIDE.md) for the complete list
2. **Prepare your VPS** - See [DEPLOYMENT_SETUP.md](DEPLOYMENT_SETUP.md) for detailed instructions
3. **Push to main** - Deployment happens automatically!

### Key Files

- **[GITHUB_SECRETS_GUIDE.md](GITHUB_SECRETS_GUIDE.md)** - Quick reference for all required GitHub secrets
- **[DEPLOYMENT_SETUP.md](DEPLOYMENT_SETUP.md)** - Complete deployment setup guide
- **[vps-deployment-check.sh](vps-deployment-check.sh)** - Script to verify VPS configuration
- **[docker-compose.prod.yml](docker-compose.prod.yml)** - Alternative docker-compose deployment method

### Deployment Flow

```
Push to main → Build Docker Image → Push to Docker Hub → SSH to VPS → Pull & Deploy
```

### Required Secrets

- Docker Hub: `DOCKER_USERNAME`, `DOCKER_TOKEN`
- VPS: `VPS_HOST`, `VPS_USERNAME`, `VPS_SSH_PRIVATE_KEY`, `VPS_PORT`
- App Config: `JWT_SECRET_KEY`, `DB_CONNECTION_STRING`, `CORS_ALLOWED_ORIGINS`, etc.

See [GITHUB_SECRETS_GUIDE.md](GITHUB_SECRETS_GUIDE.md) for details.
