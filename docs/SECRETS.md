# This configuration is for backend deploy. 

# GitHub Secrets Configuration 

### ✓ Docker Hub (2 secrets)
- [ ] `DOCKER_USERNAME` - Your Docker Hub username
- [ ] `DOCKER_TOKEN` - Docker Hub access token from https://hub.docker.com/settings/security

### ✓ VPS Connection (4 secrets)
- [ ] `VPS_HOST` - VPS IP address or hostname (e.g., `192.168.1.100`)
- [ ] `VPS_USERNAME` - SSH username (e.g., `root`, `ubuntu`, or `your-user`)
- [ ] `VPS_SSH_PRIVATE_KEY` - SSH private key (entire content including headers)
- [ ] `VPS_PORT` - SSH port number (usually `22`)

### ✓ Application Environment (6 secrets)
- [ ] `JWT_SECRET_KEY` - JWT secret (min 32 characters)
- [ ] `JWT_ISSUER` - JWT issuer (e.g., `WebApi`)
- [ ] `JWT_AUDIENCE` - JWT audience (e.g., `WebApiClient`)
- [ ] `JWT_EXPIRATION_HOURS` - Token expiration (e.g., `24`)
- [ ] `DB_CONNECTION_STRING` - Database connection string
- [ ] `CORS_ALLOWED_ORIGINS` - Allowed CORS origins (comma-separated)

## Example Values

### VPS Connection
```
VPS_HOST: 192.168.1.100
VPS_USERNAME: ubuntu
VPS_PORT: 22
VPS_SSH_PRIVATE_KEY: 
-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAABlwAAAAdzc2gtcn
...
(your full private key here)
...
-----END OPENSSH PRIVATE KEY-----
```

### JWT Configuration
```
JWT_SECRET_KEY: YourSuperSecretKeyHere_MinimumLength32Characters!
JWT_ISSUER: WebApi
JWT_AUDIENCE: WebApiClient
JWT_EXPIRATION_HOURS: 24
```

### Database Connection String
```
DB_CONNECTION_STRING: Server=your-db-server.example.com;Database=NC07WebApp;User Id=appuser;Password=YourSecurePassword123!;TrustServerCertificate=True;MultipleActiveResultSets=true
```

### CORS Configuration
```
CORS_ALLOWED_ORIGINS: https://yourdomain.com,https://www.yourdomain.com,https://app.yourdomain.com
```

## SSH Private Key Format

Your SSH private key should include the full content with headers:

```
-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAABlwAAAAdzc2gtcn
NhAAAAAwEAAQAAAYEA... (many lines) ...
-----END OPENSSH PRIVATE KEY-----
```
