CREATE TABLE journey_resolution(
    subject_key VARCHAR(64) PRIMARY KEY,
    
    identity_status VARCHAR(32) NOT NULL,
    identity_version BIGINT NOT NULL,

    customer_status VARCHAR(32) NOT NULL,
    customer_version BIGINT NOT NULL,

    onboarding_status VARCHAR(32) NOT NULL,
    onboarding_version BIGINT NOT NULL,
    
    next_action VARCHAR(64) NOT NULL,

    projection_version BIGINT NOT NULL,
    update_at TIMESTAMPZ NOT NULL
);

CREATE TABLE processed_event(
    event_id UUID PRIMARY KEY,

    event_type VARCHAR(120) NOT NULL,
    producer VARCHAR(80) NOT NULL,

    aggregate_id VARCHAR(100) NOT NULL,
    subject_key VARCHAR(64) NOT NULL,
    source_version BIGINT NOT NULL,

    occured_at TIMESTAMPZ NOT NULL,
    processed_at TIMESTAMPZ not null,

    correlation_id varchar(100),
    schema_version INTEGER NOT NULL

);