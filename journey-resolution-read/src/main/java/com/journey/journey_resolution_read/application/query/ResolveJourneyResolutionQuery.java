package com.journey.journey_resolution_read.application.query;

import java.util.Optional;

import org.springframework.stereotype.Service;

import com.journey.journey_resolution_read.port.out.JourneyResolutionProjectionRepository;

@Service
public class ResolveJourneyResolutionQuery {
    private final JourneyResolutionProjectionRepository repository;

    public ResolveJourneyResolutionQuery(JourneyResolutionProjectionRepository repository) {
        this.repository = repository;
    }

    public Optional<JourneyResolutionResult> execute(String sujectKey){
        return repository.findBySubjectKey(sujectKey)
            .map(projection -> new JourneyResolutionResult(
                projection.nextAction(),
                projection.projectionVersion(),
                projection.updatedAt()
            ));
    }

    
}
