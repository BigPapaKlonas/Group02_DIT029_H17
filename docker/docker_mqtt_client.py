import paho.mqtt.client as mqtt
import socket
import docker_container

# The callback for when the client receives a CONNACK response from the server.
def on_connect(client, userdata, flags, rc):
    print("Connected with result code "+str(rc))
    client.subscribe("processes/#", qos=2)          # Subscribes to topic with QoS 2

# The callback for when a PUBLISH message is received from the server.
def on_message(client, userdata, msg):
    print("Received: "+str(msg.payload))

    # In case received message is of specified topic, the message is past into the start_containers function
    if(msg.topic == "processes"):
        docker_container.start_containers(msg.payload.decode("utf-8"))

# 'loop_forever' automatically handles reconnecting as long as the initial connection succeeds (crashes otherwise)
# The 'connect' function retries to establish the initial connection in case it fails, thus prevents a potential crash
def connect():
    try:
        client.connect("localhost", 1883, 60)
    except socket.error as error:               # Catches socket error and retries to establish the connection
        print(error)
        print("I'll try again...")
        connect()


client = mqtt.Client()          # Creates and assigns the mqtt client
client.on_connect = on_connect  # Assigns callback to function
client.on_message = on_message
connect()
client.loop_forever()           # Makes the python script run (awaits for messages) until infinity...
