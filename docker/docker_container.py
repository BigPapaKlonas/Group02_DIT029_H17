import docker
import json
import uuid
import threading


docker_client = docker.from_env()   # Gets the default socket and configuration in the environment


class myThread(threading.Thread):
    def __init__(self, threadID, name, counter):
        threading.Thread.__init__(self)
        self.threadID = threadID
        self.name = name
        self.counter = counter

    def run(self):
        print "Starting " + self.name
        start_container(self.name)
        print "Exiting " + self.name


def start_container(container_name):
    docker_client.containers.run(image='sixonetwo/dave:latest', detach=False, name=container_name, auto_remove=False)

# Start docker containers according to JSON string (processes)
def start_containers(processes):
    try:
        decoded = json.loads(processes)             # Decodes JSON
        processes_id = uuid.uuid4().hex             # Generates unique ID for containers created from each JSON string

        for x in decoded:                                       # Iterates over the decoded JSON
            container_name = x['name'] + "_" + processes_id     # Adds processes_id to the container name
            # Starts a container, detaches and names it & removes it from daemon side when the container's process exits
            myThread(1, container_name, 1).start()

    except (ValueError, KeyError, TypeError):   # In case the JSON is either invalid or does not contain 'name'
        print("JSON format error")