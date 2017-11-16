import sys
sys.path.append('/usr/local/lib/python3.5/dist-packages/')
import paho.mqtt.client as mqtt
import container

# The callback for when the client receives a CONNACK response from the server.
def on_connect(client, userdata, flags, rc):
    print("Connected with result code "+str(rc))

    # Subscribing in on_connect() means that if we lose the connection and
    # reconnect then subscriptions will be renewed.
    client.publish("processes", "python has started!", qos=2)
    client.subscribe("processes/#", qos=2)

# The callback for when a PUBLISH message is received from the server.
def on_message(client, userdata, msg):
    print("Received: "+str(msg.payload))

    if(msg.topic == "processes"):
        container.start_containers(msg.payload.decode("utf-8"))


client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message
client.connect("52.14.146.195", 1883, 60)
client.loop_forever();