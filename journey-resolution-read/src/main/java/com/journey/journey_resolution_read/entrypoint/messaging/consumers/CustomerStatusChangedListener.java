package com.journey.journey_resolution_read.entrypoint.messaging.consumers;

import org.springframework.kafka.annotation.KafkaListener;

import com.journey.journey_resolution_read.application.projection.JourneyResolutionProjector;
import com.journey.journey_resolution_read.entrypoint.messaging.integration.IntegrationEventJsonReader;
import com.journey.journey_resolution_read.integrationevent.CustomerStatusChangedEvent;

public class CustomerStatusChangedListener {
        private final IntegrationEventJsonReader eventReader;
    private final JourneyResolutionProjector projector;
    
    public CustomerStatusChangedListener(IntegrationEventJsonReader eventReader, JourneyResolutionProjector projector) {
        this.eventReader = eventReader;
        this.projector = projector;
    }

    @KafkaListener(topics = "${journey.kafka.topics.customer}")
    public void cosume(String payload){
        var event = eventReader.read(payload, CustomerStatusChangedEvent.class);
        projector.project(event);
    }
}
