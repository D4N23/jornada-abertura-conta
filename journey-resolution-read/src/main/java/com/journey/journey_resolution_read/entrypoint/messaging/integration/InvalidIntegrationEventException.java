package com.journey.journey_resolution_read.entrypoint.messaging.integration;

public class InvalidIntegrationEventException extends RuntimeException {

    public InvalidIntegrationEventException(String msg, Throwable cause){
        super(msg, cause);
    }
}
