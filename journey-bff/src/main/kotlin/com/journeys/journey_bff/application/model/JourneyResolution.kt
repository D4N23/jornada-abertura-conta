package com.journeys.journey_bff.application.model

import java.time.Instant

data class JourneyResolution(
    val nextAction: NextAction,
    val projectionVersion: Long,
    val updateAt: Instant    
)