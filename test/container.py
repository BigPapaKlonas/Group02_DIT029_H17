import docker
import json


docker_client = docker.from_env()

def start_containers(processes):
    try:
        decoded = json.loads(processes)

        # Access data
        for x in decoded:
            name = x['name']
            if (len(x['name']) < 2):
                name = '0' + name
            print(name)
            tmp_container = docker_client.containers.run(image='hello-world', detach=True, name = name, auto_remove=True)
            print(tmp_container.logs())

    except (ValueError, KeyError, TypeError):
        print("JSON format error")




