package com.journeys.journey_bff.adapter.out.journeyread

import com.journeys.journey_bff.port.out.JourneyResolutionReader
import com.journeys.journey_bff.config.JourneyReadProperties
import com.journeys.journey_bff.error.JourneyResolutionUnavailableException
import com.journeys.journey_bff.application.model.JourneyResolution
import com.journeys.journey_bff.application.model.SubjectKey

import kotlinx.coroutines.reactor.awaitSingleOrNull
import org.springframework.stereotype.Component
import org.springframework.web.reactive.function.client.WebClient
import org.springframework.http.HttpStatus
import reactor.core.publisher.Mono

@Component
class HttpJourneyResolutionReader(
    webClientBuider: WebClient.Builder,
    properties: JourneyReadProperties
): JourneyResolutionReader{

    private val webClient = webClientBuider
        .baseUrl(properties.baseUrl)
        .build() 

    override suspend fun findBySubjecktKey(subjectKey: SubjectKey): JourneyResolution? {
        try {
            val response = webClient
                .post()
                .uri("/internal/v1/journey-resolutions/resolve")
                .bodyValue(
                    JourneyResolutionQuery(subjectKey = subjectKey.value)
                )
                .exchangeToMono{ response -> 
                    when {
                        response.statusCode().is2xxSuccessful -> {
                            response.bodyToMono(
                                JourneyResolutionResponse::class.java
                            )
                        }
                        response.statusCode() == HttpStatus.NOT_FOUND -> {
                            Mono.empty()
                        }
                        else -> {
                            response.createException().flatMap{ Mono.error(it)}
                        }
                    }
                }
                .awaitSingleOrNull()

                return response?.let {
                    JourneyResolution(
                        nextAction = it.nextAction,
                        projectionVersion = it.projectionVersion,
                        updateAt = it.updateAt
                    )
                }
        }
        catch(exeption: Exception) {
            throw JourneyResolutionUnavailableException(exeption)
        }
     }
}