package com.journey.journey_resolution_read.port.out;

import java.time.Instant;
import java.util.Optional;

import com.journey.journey_resolution_read.model.JourneyResolutionProjection;

public interface JourneyResolutionProjectionRepository {
    Optional<JourneyResolutionProjection> findBySubjectKey(String subjectKey);

    JourneyResolutionProjection lockOrCreate(String sbjectKey,Instant now);

    void save(JourneyResolutionProjection projection);
}
