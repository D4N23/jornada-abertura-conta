package com.journeys.journey_bff.config

import org.springframework.boot.context.properties.ConfigurationProperties

@ConfigurationProperties("journey.subject-key")
data class SubjectKeyProperties(
    val secret: String
)