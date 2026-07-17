package com.journey.journey_resolution_read.integrationevent;

import com.journey.journey_resolution_read.model.OnboardingStatus;

public record OnboardingStatusChangedEvent(
    IntegrationEventMetadata metadata,
    String subjectKey,
    OnboardingStatus status) {}
