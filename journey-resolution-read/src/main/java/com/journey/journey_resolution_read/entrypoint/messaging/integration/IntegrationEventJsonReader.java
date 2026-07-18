package com.journey.journey_resolution_read.entrypoint.messaging.integration;

import org.springframework.stereotype.Component;

import tools.jackson.core.JacksonException;
import tools.jackson.databind.ObjectMapper;

@Component
public class IntegrationEventJsonReader {
    
    private final ObjectMapper objectMapper;

    public IntegrationEventJsonReader(ObjectMapper objectMapper) {
        this.objectMapper = objectMapper;
    }

    public <T> T read(String payload, Class<T> eventType){
        try {
            return objectMapper.readValue(payload, eventType);
        } catch (JacksonException e) {
            throw new InvalidIntegrationEventException(
                "Invalid integration event payload", e
            ); 
        }
    }

}
