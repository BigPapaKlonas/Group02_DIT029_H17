FROM alpine:3.3

# Install the released application
COPY artifacts/diagram_executer diagram_executer


# Expose relevant ports
EXPOSE 1883 8883 8083 8084 8080 18083 4369 
EXPOSE 55000-56000

CMD /diagram_executer/bin/diagram_executer foreground