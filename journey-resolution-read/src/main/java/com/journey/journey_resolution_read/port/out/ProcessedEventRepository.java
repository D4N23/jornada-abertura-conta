package com.journey.journey_resolution_read.port.out;

import java.time.Instant;

import com.journey.journey_resolution_read.integrationevent.IntegrationEventMetadata;

public interface ProcessedEventRepository {
    
    boolean tryRegister(
        IntegrationEventMetadata metadata,
        String subjectKey,
        Instant processedAt
    );
}
