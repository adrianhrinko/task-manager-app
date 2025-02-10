#!/bin/bash

# Default to production mode
ENV_MODE="prod"

# Parse command line arguments
while [[ "$#" -gt 0 ]]; do
    case $1 in
        --dev) ENV_MODE="dev";;
        *) echo "Unknown parameter: $1"; exit 1;;
    esac
    shift
done

# Detect Mac M1/M2
if [[ "$(uname -s)" == "Darwin" && "$(uname -m)" == "arm64" ]]; then
    echo "Mac M1/M2 detected! Enabling x86 emulation..."
    export DB_PLATFORM=linux/amd64
fi

# Run Docker Compose with appropriate configuration
if [ "$ENV_MODE" = "dev" ]; then
    echo "Starting in development mode..."
    docker-compose -f docker-compose.dev.yml up --build
else
    echo "Starting in production mode..."
    docker-compose up --build
fi