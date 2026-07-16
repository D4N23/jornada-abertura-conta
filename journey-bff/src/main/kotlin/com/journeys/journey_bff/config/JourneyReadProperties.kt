package com.journeys.journey_bff.config

import org.springframework.boot.context.properties.ConfigurationProperties

@ConfigurationProperties("journey.read")
data class  JourneyReadProperties(
    val baseUrl: String
)
