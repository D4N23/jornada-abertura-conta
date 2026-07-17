package com.journey.journey_resolution_read.model;

public enum IdentySatus {
    UNKNOWN,
    ACTIVE,
    SUSPENDED,
    DISABLED;

    public boolean requiresAuthenticationFlows(){
        return this == ACTIVE || this == SUSPENDED;
    }
}
