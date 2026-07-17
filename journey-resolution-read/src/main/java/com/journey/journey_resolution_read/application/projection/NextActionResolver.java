package com.journey.journey_resolution_read.application.projection;

import org.springframework.stereotype.Component;

import com.journey.journey_resolution_read.model.JourneyResolutionProjection;
import com.journey.journey_resolution_read.model.NextAction;
import com.journey.journey_resolution_read.model.OnboardingStatus;

@Component
public class NextActionResolver {
    
    public NextAction resolve(JourneyResolutionProjection projection){
        if (projection.onboardingStatus().isResumable()) {
            return NextAction.ONBOARDING_RESUME_REQUIRED;
        }

        if (projection.onboardingStatus() == OnboardingStatus.COMPLETED) {
            return NextAction.AUTHENTICATION_REQUIRED;
        }

        if (projection.identityStatus().requiresAuthenticationFlows()) {
            return NextAction.AUTHENTICATION_REQUIRED;
        }

        if (projection.customerStatus().representsExistingRelationship()) {
            return NextAction.AUTHENTICATION_REQUIRED;
        }

        return NextAction.NEW_ONBOARDING_ALLOWED;
    }
}
