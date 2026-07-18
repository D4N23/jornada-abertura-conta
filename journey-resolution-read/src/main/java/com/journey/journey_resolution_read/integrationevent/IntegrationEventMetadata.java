package com.journey.journey_resolution_read.integrationevent;

import java.time.Instant;
import java.util.UUID;

public record IntegrationEventMetadata(
    UUID eventId,
    String eventType,
    String aggregateId,
    long subjectVersion,
    Instant occurredAt,
    String correlationId,
    String producer,
    int schemaVersion
) {
    
}
