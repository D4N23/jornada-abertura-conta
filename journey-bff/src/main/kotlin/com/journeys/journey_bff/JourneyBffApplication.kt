package com.journeys.journey_bff

import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication
import org.springframework.boot.context.properties.ConfigurationPropertiesScan

@SpringBootApplication
@ConfigurationPropertiesScan
class JourneyBffApplication

fun main(args: Array<String>) {
	runApplication<JourneyBffApplication>(*args)
}
