

# Email Example – RabbitMQ DLQ & Polly Retry (.NET 8)

This project is a .NET 8 Worker Service that processes email sending requests using RabbitMQ.
It demonstrates reliable message handling with Polly retry policies, Dead Letter Queues (DLQ), and production-ready messaging patterns.

## Architecture Overview
Message Flow

Producer publishes an EmailSendRequested message

1.Email Worker consumes the message

If processing fails:

2.Message is retried using retry logic

After maximum retry attempts:

3.Message is routed to Dead Letter Queue (DLQ)

DLQ messages can be:

Inspected manually

Reprocessed if needed
