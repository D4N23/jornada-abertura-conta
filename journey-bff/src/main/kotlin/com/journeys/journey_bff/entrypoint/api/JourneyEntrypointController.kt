package com.journeys.journey_bff.entrypoint.api

import com.journeys.journey_bff.application.ResolveEntryPointUseCase
import com.journeys.journey_bff.application.ResolveEntryPointCommand
import com.journeys.journey_bff.entrypoint.api.dto.ResolveEntryPointResponse
import com.journeys.journey_bff.entrypoint.api.dto.ResolveEntryPointRequest

import org.springframework.web.bind.annotation.RestController
import org.springframework.web.bind.annotation.RequestMapping
import org.springframework.web.bind.annotation.PostMapping
import org.springframework.web.bind.annotation.RequestBody
import jakarta.validation.Valid

@RestController
@RequestMapping("/v1/journeys")
class JourneyEntrypointController(private val resolveEntrypointUseCase: ResolveEntryPointUseCase){

    @PostMapping("/entry-point/resolve")
    suspend fun resolveEntryPoint(
        @Valid @RequestBody request: ResolveEntryPointRequest
    ): ResolveEntryPointResponse {

        val result = resolveEntrypointUseCase.execute(
            ResolveEntryPointCommand(
                rawCpf = request.identifier.value
            )
        )

        return ResolveEntryPointResponse(
            nextAction = result.nextAction
        )
    }
}