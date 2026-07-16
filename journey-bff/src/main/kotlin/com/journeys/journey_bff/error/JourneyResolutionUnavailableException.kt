package com.journeys.journey_bff.error

class JourneyResolutionUnavailableException (
    cause: Throwable? = null
) : RuntimeException(
    "Journey Resolution REad Model is unavailable",
    cause
)