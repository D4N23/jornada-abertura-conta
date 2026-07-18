package com.journey.journey_resolution_read.entrypoint.messaging.consumers;

import org.springframework.kafka.annotation.KafkaListener;
import org.springframework.stereotype.Component;

import com.journey.journey_resolution_read.application.projection.JourneyResolutionProjector;
import com.journey.journey_resolution_read.entrypoint.messaging.integration.IntegrationEventJsonReader;
import com.journey.journey_resolution_read.integrationevent.IdentityStatusChangedEvent;

@Component
public class IdentityStatusChangedListener {
    private final IntegrationEventJsonReader eventReader;
    private final JourneyResolutionProjector projector;
    
    public IdentityStatusChangedListener(IntegrationEventJsonReader eventReader, JourneyResolutionProjector projector) {
        this.eventReader = eventReader;
        this.projector = projector;
    }

    @KafkaListener(topics = "${journey.kafka.topics.identity}")
    public void cosume(String payload){
        var event = eventReader.read(payload, IdentityStatusChangedEvent.class);
        projector.project(event);
    }
}
