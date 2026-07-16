package com.journeys.journey_bff.entrypoint.api.dto

import jakarta.validation.constraints.NotBlank

data class JourneyIdentifierRequest(
    val type: IdentifierType,    
    @field:NotBlank val value: String
)