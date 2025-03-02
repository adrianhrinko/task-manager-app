import { exec } from 'child_process';

// Define services to generate clients for
const services = [
    ['TaskManager', '../backend/task-manager-swagger.json'],
];

// Generate clients for each service
services.forEach(([serviceName, swaggerPath]) => {
    const command = `./node_modules/.bin/nswag openapi2tsclient /input:${swaggerPath} /output:src/api/${serviceName}Client.ts /template:Axios /className:${serviceName}Client /generateClientClasses:true /generateClientInterfaces:true /operationGenerationMode:SingleClientFromOperationId`;

    exec(command, (error, stdout, stderr) => {
        if (error) {
            console.error(`Error generating client for ${serviceName}: ${error}`);
            return;
        }
        console.log(`API client for ${serviceName} generated successfully`);
    });
});