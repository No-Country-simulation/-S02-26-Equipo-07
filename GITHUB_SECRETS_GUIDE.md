# GitHub Secrets Configuration - Quick Reference

## Overview
This document provides a quick checklist of all GitHub Secrets that need to be configured for the VPS deployment to work.

## Location
Configure these in: **GitHub Repository** → **Settings** → **Secrets and variables** → **Actions** → **New repository secret**

## Required Secrets Checklist

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

**Important Notes:**
- Include the `-----BEGIN` and `-----END` lines
- Copy the entire key including line breaks
- Don't add extra spaces or formatting

## How to Get Your SSH Private Key

If you don't have one yet:

```bash
# On your local machine, generate a new key pair
ssh-keygen -t rsa -b 4096 -C "your_email@example.com"

# This creates:
# - Private key: ~/.ssh/id_rsa (keep this secret!)
# - Public key: ~/.ssh/id_rsa.pub (copy to VPS)

# Copy public key to VPS
ssh-copy-id username@vps-ip

# Or manually:
cat ~/.ssh/id_rsa.pub
# Copy the output and add to VPS ~/.ssh/authorized_keys

# Get private key content for GitHub secret
cat ~/.ssh/id_rsa
# Copy the entire output including headers
```

## Deployment Flow

Once all secrets are configured:

1. **Developer pushes code** to `main` branch (backend changes)
2. **GitHub Actions triggers** automatically
3. **Builds Docker image** and pushes to Docker Hub
4. **Connects to VPS** via SSH
5. **Pulls latest image** from Docker Hub
6. **Stops old container** (if running)
7. **Starts new container** with all environment variables
8. **Application is live** on VPS

## Testing the Setup

After configuring all secrets:

1. Make a small change to backend code (e.g., add a comment)
2. Push to `main` branch
3. Go to **Actions** tab in GitHub
4. Watch the deployment process
5. Check if deployment succeeds
6. Test your API at `http://your-vps-ip/`

## Troubleshooting Quick Tips

### SSH Connection Failed
- Verify VPS_HOST is correct IP/hostname
- Check VPS_PORT (usually 22)
- Ensure private key format is correct (with headers)
- Verify VPS allows SSH connections

### Docker Pull Failed
- Check DOCKER_USERNAME and DOCKER_TOKEN
- Verify token has read permissions
- Ensure image name is correct in workflow

### Container Won't Start
- Check docker logs: `docker logs ecomerse_backend`
- Verify DB_CONNECTION_STRING is correct
- Ensure database is accessible from VPS
- Check all environment variables are set

### Database Connection Error
- Test database connectivity from VPS: `telnet db-server 1433`
- Verify database allows connections from VPS IP
- Check connection string format
- Ensure database user has proper permissions

## Security Reminders

- ✓ Never commit secrets to code
- ✓ Use strong, unique passwords
- ✓ Rotate credentials regularly
- ✓ Enable firewall on VPS
- ✓ Use HTTPS in production
- ✓ Keep systems updated
- ✓ Monitor access logs

## Next Steps

After successful deployment:

1. ✓ Test API endpoints
2. ✓ Set up HTTPS (Let's Encrypt + Nginx)
3. ✓ Configure monitoring
4. ✓ Set up database backups
5. ✓ Configure domain name (if applicable)
6. ✓ Set up logging aggregation
7. ✓ Create health check endpoint
8. ✓ Configure alerting

## Support Resources

- Full guide: See `DEPLOYMENT_SETUP.md`
- GitHub Actions logs: Repository → Actions tab
- VPS logs: `ssh vps-ip` → `docker logs ecomerse_backend`
