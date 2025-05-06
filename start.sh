#!/bin/bash

# Default to production mode and full stack
ENV_MODE="prod"
SERVICES="all"

# Parse command line arguments
while [[ "$#" -gt 0 ]]; do
    case $1 in
        --dev) ENV_MODE="dev";;
        --backend) SERVICES="backend";;
        --frontend) SERVICES="frontend";;
        *) echo "Unknown parameter: $1"; exit 1;;
    esac
    shift
done

# Detect Mac M1/M2
if [[ "$(uname -s)" == "Darwin" && "$(uname -m)" == "arm64" ]]; then
    echo "Mac M1/M2 detected! Enabling x86 emulation..."
    export DB_PLATFORM=linux/amd64
fi

# Define service groups
BACKEND_SERVICES="keycloak db-server rabbitmq proxy task-service user-service"
FRONTEND_SERVICES="frontend proxy"  # Including proxy as frontend needs it

# Run Docker Compose with appropriate configuration
if [ "$ENV_MODE" = "dev" ]; then
    echo "Starting in development mode..."
    case $SERVICES in
        "backend")
            echo "Starting backend services only..."
            docker-compose -f docker-compose.dev.yml up --build $BACKEND_SERVICES
            ;;
        "frontend")
            echo "Starting frontend services only..."
            docker-compose -f docker-compose.dev.yml up --build $FRONTEND_SERVICES
            ;;
        *)
            echo "Starting all services..."
            docker-compose -f docker-compose.dev.yml up --build
            ;;
    esac
else
    echo "Starting in production mode..."
    case $SERVICES in
        "backend")
            echo "Starting backend services only..."
            docker-compose up --build $BACKEND_SERVICES
            ;;
        "frontend")
            echo "Starting frontend services only..."
            docker-compose up --build $FRONTEND_SERVICES
            ;;
        *)
            echo "Starting all services..."
            docker-compose up --build
            ;;
    esac
fi