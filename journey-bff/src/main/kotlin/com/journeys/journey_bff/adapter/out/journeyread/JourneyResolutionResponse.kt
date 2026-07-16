package com.journeys.journey_bff.adapter.out.journeyread

import com.journeys.journey_bff.application.model.NextAction
import java.time.Instant

data class JourneyResolutionResponse(
    val nextAction: NextAction,
    val projectionVersion: Long,
    val updateAt: Instant
)