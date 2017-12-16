FROM alpine:3.3

# Install the released application
COPY artifacts/pubsub_ssdchat pubsub_ssdchat


# Expose relevant ports
EXPOSE 1883 8883 8083 8084 8080 18083 4369 
EXPOSE 55000-56000

CMD /pubsub_ssdchat/bin/pubsub_ssdchat foreground