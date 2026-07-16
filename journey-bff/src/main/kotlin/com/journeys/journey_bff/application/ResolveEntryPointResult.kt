package com.journeys.journey_bff.application

import com.journeys.journey_bff.application.model.NextAction

data class ResolveEntryPointResult(
    val nextAction: NextAction
)