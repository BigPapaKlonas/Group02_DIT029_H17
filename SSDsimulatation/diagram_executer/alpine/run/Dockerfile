FROM alpine:3.3

# Install the released application
COPY artifacts/default default


# Expose relevant ports
EXPOSE 1883 8883 8083 8084 8080 18083 4369 
EXPOSE 55000-56000

CMD /default/bin/default foreground