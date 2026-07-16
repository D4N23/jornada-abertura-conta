package com.journeys.journey_bff.port.out

import com.journeys.journey_bff.application.model.JourneyResolution
import com.journeys.journey_bff.application.model.SubjectKey

interface JourneyResolutionReader {
    suspend fun findBySubjecktKey(subjectKey: SubjectKey): JourneyResolution?
}