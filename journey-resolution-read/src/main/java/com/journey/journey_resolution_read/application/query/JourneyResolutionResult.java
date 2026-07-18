package com.journey.journey_resolution_read.application.query;

import java.time.Instant;

import com.journey.journey_resolution_read.model.NextAction;

public record JourneyResolutionResult(
    NextAction nextAction,
    long projectionVersion,
    Instant updateAt
) {
}
