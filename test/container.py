import docker
import json
import uuid


docker_client = docker.from_env()   # getting the default socket and configuration in our environment

# Start docker containers according to JSON string (processes)
def start_containers(processes):
    try:
        decoded = json.loads(processes)             # Decodes JSON
        processes_id = uuid.uuid4().hex             # Generates unique ID for containers created from this JSON string

        for x in decoded:                                       # Iterates over the decoded JSON
            container_name = x['name'] + "_" + processes_id     # Adds processes_id to the container name
            # Starts a container, detaches and names it & removes it from daemon side when the container's process exits
            docker_client.containers.run(image='hello-world', detach=True, name=container_name, auto_remove=True)

    except (ValueError, KeyError, TypeError):   # In case the JSON is either invalid or does not contain 'name'
        print("JSON format error")
