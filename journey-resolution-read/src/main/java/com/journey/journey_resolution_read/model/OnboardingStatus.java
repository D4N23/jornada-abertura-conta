package com.journey.journey_resolution_read.model;

public enum OnboardingStatus {
    
    NONE,
    IN_PROGRESS,
    COMPLETED,
    REJECTED,
    EXPIRED;

    public boolean isResumable() {
        return this == IN_PROGRESS;
    }
}
