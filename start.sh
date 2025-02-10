#!/bin/bash

# Detect Mac M1/M2
if [[ "$(uname -s)" == "Darwin" && "$(uname -m)" == "arm64" ]]; then
    echo "Mac M1/M2 detected! Enabling x86 emulation..."
    export DB_PLATFORM=linux/amd64
fi

# Run Docker Compose
docker-compose up --build