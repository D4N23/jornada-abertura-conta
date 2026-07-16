export type IdentifierType = "CPF"

export type JourneyNextAction = 
    | "AUTHENTICATION_REQUIRED"
    | "ONBOARDING_RESUME_REQUIRED"
    | "NEW_ONBOARDING_ALLOWED"
    | "ONBOARDING_UNAVAILABLE";

export interface JourneyResolutionRequest{
    identifier: {
        type: IdentifierType,
        value: string
    }
}

export interface JourneyResolutionResponse{
    nextAction: JourneyNextAction
}
