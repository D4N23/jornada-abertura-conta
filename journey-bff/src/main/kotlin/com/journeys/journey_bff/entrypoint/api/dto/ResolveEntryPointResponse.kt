package com.journeys.journey_bff.entrypoint.api.dto

import com.journeys.journey_bff.application.model.NextAction

data class ResolveEntryPointResponse(val nextAction: NextAction)