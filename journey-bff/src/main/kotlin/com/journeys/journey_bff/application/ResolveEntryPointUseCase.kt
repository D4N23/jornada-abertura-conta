package com.journeys.journey_bff.application

interface ResolveEntryPointUseCase{
    suspend fun execute( command: ResolveEntryPointCommand): ResolveEntryPointResult
}