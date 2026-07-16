package com.journeys.journey_bff.config

import org.springframework.context.annotation.Bean
import org.springframework.context.annotation.Configuration
import org.springframework.web.reactive.function.client.WebClient

@Configuration
class HttpClientConfiguration {

    @Bean
    fun webClientBuilder(): WebClient.Builder = WebClient.builder()
}
