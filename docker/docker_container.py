import docker
import json
import uuid



docker_client = docker.from_env()   # Gets the default socket and configuration in the environment

# Start docker containers according to JSON string (processes)
def start_containers(request):

    containers = int(request)
    
    for x in range(containers):                                       # Iterates over the decoded JSON
        # Starts a container, detaches & removes it from daemon side when the container's process exits
        docker_client.containers.run(image='sixonetwo/dave:latest', detach=True, auto_remove=True)

