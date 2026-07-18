package com.journey.journey_resolution_read.application.projection;

import java.time.Clock;
import java.time.Instant;

import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.journey.journey_resolution_read.integrationevent.CustomerStatusChangedEvent;
import com.journey.journey_resolution_read.integrationevent.IdentityStatusChangedEvent;
import com.journey.journey_resolution_read.integrationevent.OnboardingStatusChangedEvent;
import com.journey.journey_resolution_read.port.out.JourneyResolutionProjectionRepository;
import com.journey.journey_resolution_read.port.out.ProcessedEventRepository;

@Service
public class JourneyResolutionProjector {
    
    private final ProcessedEventRepository processedEventRepository;
    private final JourneyResolutionProjectionRepository projectionRepository;
    private final NextActionResolver nextActionResolver;
    private final Clock clock;
    
    public JourneyResolutionProjector(ProcessedEventRepository processedEventRepository,
            JourneyResolutionProjectionRepository projectionRepository, NextActionResolver nextActionResolver,
            Clock clock) {
        this.processedEventRepository = processedEventRepository;
        this.projectionRepository = projectionRepository;
        this.nextActionResolver = nextActionResolver;
        this.clock = clock;
    }

    @Transactional
    public void project(IdentityStatusChangedEvent event) {
        Instant now = clock.instant();

        if (!processedEventRepository.tryRegister(event.metadata(), event.subjectKey(), now)) {
            return;
        }

        var current = projectionRepository.lockOrCreate(event.subjectKey(), now);

        if (event.metadata().subjectVersion() <= current.identityVersion()) {
            return;
        }

        var candidate = current.withIdentity(event.status(), event.metadata().subjectVersion());

        var updated = candidate.resolvedAs(nextActionResolver.resolve(candidate), now);

        projectionRepository.save(updated);
    }

    @Transactional
    public void project(CustomerStatusChangedEvent event) {
        Instant now = clock.instant();

        if (!processedEventRepository.tryRegister(event.metadata(), event.subjectKey(), now)) {
            return;
        }

        var current = projectionRepository.lockOrCreate(event.subjectKey(), now);

        if (event.metadata().subjectVersion() <= current.customerVersion()) {
            return;
        }

        var candidate = current.withCustomer(event.status(), event.metadata().subjectVersion());

        var updated = candidate.resolvedAs(nextActionResolver.resolve(candidate),now);

        projectionRepository.save(updated);
    }

    @Transactional
    public void project(OnboardingStatusChangedEvent event) {
        Instant now = clock.instant();

        if (!processedEventRepository.tryRegister(event.metadata(), event.subjectKey(), now)) {
            return;
        }

        var current = projectionRepository.lockOrCreate(event.subjectKey(), now);

        if (event.metadata().subjectVersion() <= current.onboardingVersion()) {
            return;
        }

        var candidate = current.withOnboarding(event.status(), event.metadata().subjectVersion());

        var updated = candidate.resolvedAs(nextActionResolver.resolve(candidate), now);

        projectionRepository.save(updated);
    }
}
