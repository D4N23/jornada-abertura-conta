package com.journey.journey_resolution_read.entrypoint.api.dto;

import java.time.Instant;

import com.journey.journey_resolution_read.model.NextAction;

public record JourneyResolutionResponse(
    NextAction nextAction,
    long projectionVersion,
    Instant updateAt){
}
