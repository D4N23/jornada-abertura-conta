package com.journey.journey_resolution_read.entrypoint.api.dto;

import jakarta.validation.constraints.NotBlank;

public record JourneyResolutionQuery(
    @NotBlank
    String subjectKey
) { 
}
