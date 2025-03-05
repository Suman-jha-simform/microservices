# Producer
This adds orders into the database and generates a message in the queue with order details to be consumed by consumer api.

# Consumer
It consumes the message published by producer in the queue and sends backs an acknowledgement after consumption.
