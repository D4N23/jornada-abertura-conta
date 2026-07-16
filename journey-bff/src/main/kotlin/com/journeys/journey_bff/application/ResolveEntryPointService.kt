package com.journeys.journey_bff.application

import com.journeys.journey_bff.application.model.NextAction
import com.journeys.journey_bff.port.out.JourneyResolutionReader
import com.journeys.journey_bff.port.out.SubjectKeyFactory
import org.springframework.stereotype.Service

@Service
class ResolveEntryPointService(
        private val subjectKeyFactory: SubjectKeyFactory,
        private val journeyResolutionReader: JourneyResolutionReader 
    ) : ResolveEntryPointUseCase {
        
        override suspend fun execute(command: ResolveEntryPointCommand):ResolveEntryPointResult{
            val subjectKey = subjectKeyFactory.fromCpf(command.rawCpf)
            val resolution = journeyResolutionReader.findBySubjecktKey(subjectKey)
            
            return ResolveEntryPointResult(
                nextAction = resolution?.nextAction ?: NextAction.NEW_ONBOARDING_ALLOWED 
            )
        }
    }