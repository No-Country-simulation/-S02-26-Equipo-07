#!/bin/bash

# VPS Deployment Verification Script
# This script helps verify that your VPS is properly configured for automatic deployment

echo "========================================="
echo "VPS Deployment Configuration Checker"
echo "========================================="
echo ""

# Color codes for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Track if any issues found
ISSUES_FOUND=0

# Function to check command existence
check_command() {
    if command -v $1 &> /dev/null; then
        echo -e "${GREEN}✓${NC} $1 is installed"
        return 0
    else
        echo -e "${RED}✗${NC} $1 is NOT installed"
        ISSUES_FOUND=$((ISSUES_FOUND + 1))
        return 1
    fi
}

# Function to check port
check_port() {
    if netstat -tuln | grep -q ":$1 "; then
        echo -e "${YELLOW}⚠${NC} Port $1 is already in use"
        echo "    Current process using port $1:"
        netstat -tuln | grep ":$1 " || ss -tuln | grep ":$1 "
    else
        echo -e "${GREEN}✓${NC} Port $1 is available"
    fi
}

echo "1. Checking Prerequisites..."
echo "----------------------------"

# Check Docker
check_command docker

# Check if Docker service is running
if systemctl is-active --quiet docker 2>/dev/null || service docker status > /dev/null 2>&1; then
    echo -e "${GREEN}✓${NC} Docker service is running"
else
    echo -e "${RED}✗${NC} Docker service is NOT running"
    ISSUES_FOUND=$((ISSUES_FOUND + 1))
fi

# Check if current user can run Docker without sudo
if docker ps > /dev/null 2>&1; then
    echo -e "${GREEN}✓${NC} Current user can run Docker commands"
else
    echo -e "${YELLOW}⚠${NC} Current user cannot run Docker without sudo"
    echo "    Run: sudo usermod -aG docker \$USER"
    echo "    Then log out and log back in"
fi

echo ""
echo "2. Checking Network Configuration..."
echo "-------------------------------------"

# Check SSH port
if ss -tuln | grep -q ":22 " || netstat -tuln | grep -q ":22 "; then
    echo -e "${GREEN}✓${NC} SSH port (22) is open"
else
    echo -e "${YELLOW}⚠${NC} SSH port (22) may not be accessible"
fi

# Check HTTP port
check_port 80

# Check HTTPS port
check_port 443

echo ""
echo "3. Checking Docker Hub Access..."
echo "---------------------------------"

# Try to access Docker Hub
if docker search hello-world --limit 1 > /dev/null 2>&1; then
    echo -e "${GREEN}✓${NC} Can access Docker Hub"
else
    echo -e "${YELLOW}⚠${NC} Cannot access Docker Hub (may need login)"
fi

echo ""
echo "4. Checking Existing Containers..."
echo "-----------------------------------"

# Check if backend container exists
if docker ps -a | grep -q ecomerse_backend; then
    echo -e "${GREEN}✓${NC} Found ecomerse_backend container"
    
    # Check if it's running
    if docker ps | grep -q ecomerse_backend; then
        echo -e "${GREEN}✓${NC} Container is running"
        
        # Show container info
        echo ""
        echo "Container details:"
        docker ps --filter "name=ecomerse_backend" --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"
        
        # Check logs for errors
        echo ""
        echo "Recent logs (last 10 lines):"
        docker logs --tail 10 ecomerse_backend 2>&1
        
    else
        echo -e "${YELLOW}⚠${NC} Container exists but is not running"
        echo "    Status:"
        docker ps -a --filter "name=ecomerse_backend" --format "table {{.Names}}\t{{.Status}}"
    fi
else
    echo -e "${YELLOW}⚠${NC} No ecomerse_backend container found (will be created on first deployment)"
fi

echo ""
echo "5. Checking Firewall Configuration..."
echo "--------------------------------------"

# Check UFW (Ubuntu/Debian)
if command -v ufw &> /dev/null; then
    if ufw status | grep -q "Status: active"; then
        echo -e "${GREEN}✓${NC} UFW firewall is active"
        echo ""
        echo "Firewall rules:"
        ufw status | grep -E "(22|80|443)"
    else
        echo -e "${YELLOW}⚠${NC} UFW firewall is inactive"
    fi
fi

# Check firewalld (CentOS/RHEL)
if command -v firewall-cmd &> /dev/null; then
    if firewall-cmd --state | grep -q "running"; then
        echo -e "${GREEN}✓${NC} firewalld is active"
        echo ""
        echo "Firewall rules:"
        firewall-cmd --list-services
    else
        echo -e "${YELLOW}⚠${NC} firewalld is inactive"
    fi
fi

echo ""
echo "6. Testing Database Connectivity..."
echo "------------------------------------"

# This would need the actual database connection details
echo -e "${YELLOW}ℹ${NC} Database connectivity test requires DB_CONNECTION_STRING"
echo "    You can test manually with:"
echo "    telnet <database-host> 1433"
echo "    or: nc -zv <database-host> 1433"

echo ""
echo "7. Disk Space Check..."
echo "-----------------------"

df -h / | tail -1 | awk '{
    used = substr($5, 1, length($5)-1);
    if (used > 80) {
        printf "\033[0;31m✗\033[0m Disk usage is high: %s\n", $5;
    } else if (used > 60) {
        printf "\033[1;33m⚠\033[0m Disk usage: %s\n", $5;
    } else {
        printf "\033[0;32m✓\033[0m Disk usage: %s\n", $5;
    }
}'

echo ""
echo "8. Docker Images..."
echo "--------------------"

# Show backend images
if docker images | grep -q "ecomerse_backend"; then
    echo "Backend images found:"
    docker images | grep "ecomerse_backend" || echo "None"
else
    echo -e "${YELLOW}⚠${NC} No backend images found yet"
fi

echo ""
echo "========================================="
echo "Summary"
echo "========================================="

if [ $ISSUES_FOUND -eq 0 ]; then
    echo -e "${GREEN}✓ All checks passed!${NC}"
    echo "Your VPS is ready for automatic deployment."
else
    echo -e "${RED}✗ Found $ISSUES_FOUND issue(s)${NC}"
    echo "Please resolve the issues above before deploying."
fi

echo ""
echo "Next Steps:"
echo "1. Configure GitHub Secrets (see GITHUB_SECRETS_GUIDE.md)"
echo "2. Push changes to main branch"
echo "3. Monitor deployment in GitHub Actions"
echo "4. Verify deployment: curl http://localhost"
echo ""
echo "For manual deployment, run:"
echo "  docker pull your-username/ecomerse_backend:main"
echo "  docker stop ecomerse_backend || true"
echo "  docker rm ecomerse_backend || true"
echo "  docker run -d --name ecomerse_backend --restart unless-stopped \\"
echo "    -p 80:80 \\"
echo "    -e JWT_SECRET_KEY='...' \\"
echo "    -e DB_CONNECTION_STRING='...' \\"
echo "    your-username/ecomerse_backend:main"
echo ""
