package com.journeys.journey_bff.entrypoint.api.dto

import jakarta.validation.Valid

data class ResolveEntryPointRequest(
    @field:Valid val identifier: JourneyIdentifierRequest
)