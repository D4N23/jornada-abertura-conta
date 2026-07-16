package com.journeys.journey_bff.config

import org.springframework.web.reactive.config.WebFluxConfigurer
import org.springframework.web.reactive.config.CorsRegistry
import org.springframework.context.annotation.Configuration

@Configuration
class WebCorsConfig : WebFluxConfigurer{

    override fun addCorsMappings(registry: CorsRegistry){
        registry
            .addMapping("/v1/**")
            .allowedOrigins("*")
            .allowedMethods("POST", "OPTIONS")
            .allowedHeaders("Content-Type")
    }
}