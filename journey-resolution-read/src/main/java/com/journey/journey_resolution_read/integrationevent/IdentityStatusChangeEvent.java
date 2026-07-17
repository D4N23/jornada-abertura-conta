package com.journey.journey_resolution_read.integrationevent;

import com.journey.journey_resolution_read.model.IdentityStatus;

public record IdentityStatusChangeEvent(
    IntegrationEventMetadata metadata,
    String subjectKey,
    IdentityStatus status
) {}
