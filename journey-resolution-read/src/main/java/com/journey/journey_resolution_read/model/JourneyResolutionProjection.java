package com.journey.journey_resolution_read.model;

import java.time.Instant;

public record JourneyResolutionProjection(
    String subjectKey,
    
    IdentityStatus identityStatus,
    long identityVersion,

    CustomerStatus customerStatus,
    long customerVersion,

    OnboardingStatus onboardingStatus,
    long onboardingVersion,

    NextAction nextAction,

    long projectionVersion,
    Instant updatedAt
) {
    
    public JourneyResolutionProjection withIdentity(
        IdentityStatus status,
        long sourceVersion
    ){
        return new JourneyResolutionProjection(
                subjectKey, 
                status, 
                sourceVersion, 
                customerStatus, 
                customerVersion, 
                onboardingStatus, 
                onboardingVersion, 
                nextAction, 
                projectionVersion, 
                updatedAt);
    }

    public JourneyResolutionProjection withCustomer(
        CustomerStatus status,
        long sourceVersion
    ){
        return new JourneyResolutionProjection(
            subjectKey, 
            identityStatus, 
            identityVersion, 
            status, 
            sourceVersion, 
            onboardingStatus, 
            onboardingVersion, 
            nextAction, 
            projectionVersion, 
            updatedAt);
    }

    public JourneyResolutionProjection withOnboarding(
        OnboardingStatus status,
        long sourceVersion
    ){
        return new JourneyResolutionProjection(
            subjectKey, 
            identityStatus, 
            identityVersion, 
            customerStatus, 
            customerVersion, 
            status, 
            sourceVersion, 
            nextAction, 
            projectionVersion, 
            updatedAt);
    }

    public JourneyResolutionProjection resolvedAs(
        NextAction resolvedNextAction,
        Instant resolvedAt
    ){
        return new JourneyResolutionProjection(
            subjectKey, 
            identityStatus, 
            identityVersion, 
            customerStatus, 
            customerVersion, 
            onboardingStatus, 
            onboardingVersion, 
            resolvedNextAction, 
            projectionVersion + 1, 
            resolvedAt);
    }
}
