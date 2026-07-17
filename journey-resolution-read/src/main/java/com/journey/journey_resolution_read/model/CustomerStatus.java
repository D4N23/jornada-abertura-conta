package com.journey.journey_resolution_read.model;

public enum CustomerStatus {
    UNKNOWN,
    ACTIVE,
    SUSPENDED,
    CLOSED;

    public boolean representsExistingRelationship(){
        return this == ACTIVE || this == SUSPENDED;
    }
}
