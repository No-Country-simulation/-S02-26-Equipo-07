# VPS Deployment Setup Guide

This guide explains how to configure the automatic deployment of the backend application to your VPS after building and pushing the Docker image to Docker Hub.

## How It Works

The CI/CD pipeline (`docker-build.yml`) now performs the following steps:

1. **Build & Push**: Builds the Docker image and pushes it to Docker Hub
2. **Deploy to VPS**: Automatically connects to your VPS via SSH and:
   - Pulls the latest image from Docker Hub
   - Stops and removes the old container
   - Starts a new container with all required environment variables
   - Cleans up old Docker images

## Required GitHub Secrets

You need to configure the following secrets in your GitHub repository:

### Navigate to: `Settings` → `Secrets and variables` → `Actions` → `New repository secret`

### Docker Hub Credentials
- **`DOCKER_USERNAME`**: Your Docker Hub username
- **`DOCKER_TOKEN`**: Your Docker Hub access token (create at https://hub.docker.com/settings/security)

### VPS Connection Details
- **`VPS_HOST`**: Your VPS IP address or hostname (e.g., `192.168.1.100` or `myserver.example.com`)
- **`VPS_USERNAME`**: SSH username for your VPS (e.g., `root` or `ubuntu`)
- **`VPS_SSH_PRIVATE_KEY`**: Your SSH private key for connecting to the VPS
- **`VPS_PORT`**: SSH port (default: `22`, or your custom port)

### Application Environment Variables
- **`JWT_SECRET_KEY`**: Secret key for JWT token generation (minimum 32 characters)
- **`JWT_ISSUER`**: JWT issuer name (e.g., `WebApi`)
- **`JWT_AUDIENCE`**: JWT audience name (e.g., `WebApiClient`)
- **`JWT_EXPIRATION_HOURS`**: JWT token expiration in hours (e.g., `24`)
- **`DB_CONNECTION_STRING`**: Database connection string (e.g., `Server=your-db-server;Database=NC07WebApp;User Id=appuser;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=true`)
- **`CORS_ALLOWED_ORIGINS`**: Comma-separated list of allowed CORS origins (e.g., `https://yourdomain.com,https://www.yourdomain.com`)

## VPS Prerequisites

Make sure your VPS has the following installed and configured:

### 1. Install Docker
```bash
# Update package index
sudo apt-get update

# Install Docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Add your user to the docker group (optional, to run docker without sudo)
sudo usermod -aG docker $USER

# Verify installation
docker --version
```

### 2. Configure SSH Access
Make sure SSH is enabled and you can connect with your private key:
```bash
# Test SSH connection from your local machine
ssh -i /path/to/your/private-key username@vps-ip
```

### 3. Open Required Ports
Make sure port 80 (HTTP) is open in your VPS firewall:
```bash
# For UFW (Ubuntu)
sudo ufw allow 80/tcp
sudo ufw allow 22/tcp  # SSH
sudo ufw enable

# For firewalld (CentOS/RHEL)
sudo firewall-cmd --permanent --add-service=http
sudo firewall-cmd --permanent --add-service=ssh
sudo firewall-cmd --reload
```

## Deployment Process

Once all secrets are configured, the deployment happens automatically:

1. Push code changes to the `main` branch that affect the `backend/**` directory
2. GitHub Actions will:
   - Build the Docker image
   - Push it to Docker Hub
   - SSH into your VPS
   - Pull and deploy the new image

## Monitoring Deployments

### View Deployment Logs
1. Go to your GitHub repository
2. Click on `Actions` tab
3. Select the latest workflow run
4. Click on the `build-and-push` job
5. Expand the `Deploy to VPS` step to see deployment logs

### Check Running Container on VPS
```bash
# SSH into your VPS
ssh username@vps-ip

# Check if container is running
docker ps | grep ecomerse_backend

# View container logs
docker logs ecomerse_backend

# Follow logs in real-time
docker logs -f ecomerse_backend
```

## Troubleshooting

### Container Not Starting
Check the logs:
```bash
docker logs ecomerse_backend
```

Common issues:
- Database connection string is incorrect
- Database server is not accessible from VPS
- Environment variables are missing or incorrect

### Database Connection Issues
Make sure:
1. Database server allows connections from your VPS IP
2. Connection string is correct and includes `TrustServerCertificate=True` for SQL Server
3. Firewall allows database port (default 1433 for SQL Server)

### SSH Connection Failed
Verify:
1. VPS IP address and port are correct
2. SSH private key is properly formatted (include `-----BEGIN ... KEY-----` headers)
3. VPS username has docker permissions
4. SSH port is open in firewall

### Port Already in Use
If port 80 is already in use, you can change it in the workflow file:
```yaml
-p 8080:80  # Maps VPS port 8080 to container port 80
```

## Manual Deployment

If you need to manually deploy or restart the container:

```bash
# SSH into your VPS
ssh username@vps-ip

# Login to Docker Hub
docker login -u your-username

# Pull the latest image
docker pull your-username/ecomerse_backend:main

# Stop and remove old container
docker stop ecomerse_backend
docker rm ecomerse_backend

# Run new container
docker run -d \
  --name ecomerse_backend \
  --restart unless-stopped \
  -p 80:80 \
  -e JWT_SECRET_KEY="your-secret-key" \
  -e JWT_ISSUER="WebApi" \
  -e JWT_AUDIENCE="WebApiClient" \
  -e JWT_EXPIRATION_HOURS="24" \
  -e DB_CONNECTION_STRING="Server=...;Database=..." \
  -e CORS_ALLOWED_ORIGINS="https://yourdomain.com" \
  -e ASPNETCORE_ENVIRONMENT="Production" \
  your-username/ecomerse_backend:main
```

## Additional Configuration

### HTTPS Setup (Recommended for Production)

For production, you should use HTTPS. Here are two options:

#### Option 1: Reverse Proxy with Nginx
1. Install Nginx on your VPS
2. Configure SSL certificate (use Let's Encrypt)
3. Set up Nginx as reverse proxy to your Docker container

#### Option 2: Use Traefik
1. Set up Traefik as reverse proxy
2. Configure automatic SSL with Let's Encrypt
3. Traefik will handle HTTPS termination

### Database Backup
Make sure to set up regular backups of your database server.

### Monitoring
Consider setting up monitoring tools like:
- Docker stats: `docker stats ecomerse_backend`
- Application logs: `docker logs ecomerse_backend`
- Health check endpoint monitoring

## Security Best Practices

1. **Never commit secrets to the repository**
2. **Use strong passwords and keys**
3. **Keep Docker and VPS system updated**
4. **Use firewall to restrict access**
5. **Enable HTTPS for production**
6. **Regularly rotate credentials**
7. **Monitor access logs**

## Support

If you encounter any issues:
1. Check the GitHub Actions logs
2. Check the Docker container logs on VPS
3. Verify all secrets are correctly configured
4. Ensure VPS prerequisites are met
