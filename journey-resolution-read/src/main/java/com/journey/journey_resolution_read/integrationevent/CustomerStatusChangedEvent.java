package com.journey.journey_resolution_read.integrationevent;

import com.journey.journey_resolution_read.model.CustomerStatus;

public record CustomerStatusChangedEvent(
    IntegrationEventMetadata metadata,
    String subjectKey,
    CustomerStatus status
) {}
