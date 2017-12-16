import docker
import json
import uuid

docker_client = docker.from_env()   # Gets the default socket and configuration in the environment

# Start request number of docker containers
def start_containers(request):

    containers = int(request)

    for x in range(containers): # Interates over the requested number of containers
        # Starts a container with the latest image from dockerhub and detaches it
        docker_client.containers.run('sixonetwo/dave:latest', detach=True)


