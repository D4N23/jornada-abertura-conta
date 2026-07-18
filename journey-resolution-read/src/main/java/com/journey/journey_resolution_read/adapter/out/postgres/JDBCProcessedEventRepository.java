package com.journey.journey_resolution_read.adapter.out.postgres;

import java.time.Instant;
import java.time.OffsetDateTime;
import java.time.ZoneOffset;

import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.namedparam.NamedParameterJdbcTemplate;

import com.journey.journey_resolution_read.integrationevent.IntegrationEventMetadata;
import com.journey.journey_resolution_read.port.out.ProcessedEventRepository;

public class JDBCProcessedEventRepository implements ProcessedEventRepository{


    private final NamedParameterJdbcTemplate template;

    public JDBCProcessedEventRepository(NamedParameterJdbcTemplate template) {
        this.template = template;
    }

    @Override
    public boolean tryRegister(IntegrationEventMetadata metadata, String subjectKey, Instant processedAt) {
        
        var parameters = new MapSqlParameterSource()
            .addValue("eventId", metadata.eventId())
            .addValue("eventType", metadata.eventType())
            .addValue("producer", metadata.producer())
            .addValue("aggregateId", metadata.aggregateId())
            .addValue("subjectKey", subjectKey)
            .addValue("sourceVersion",metadata.subjectVersion())
            .addValue(
                "occurredAt",
                OffsetDateTime.ofInstant(
                    metadata.occurredAt(),
                    ZoneOffset.UTC))
            .addValue("processedAt",OffsetDateTime.ofInstant(processedAt,ZoneOffset.UTC))
            .addValue("correlationId",metadata.correlationId())
            .addValue("schemaVersion",metadata.schemaVersion());

            int insertedRows = template.update(
            """
            INSERT INTO processed_event (
                event_id,
                event_type,
                producer,
                aggregate_id,
                subject_key,
                source_version,
                occurred_at,
                processed_at,
                correlation_id,
                schema_version
            )
            VALUES (
                :eventId,
                :eventType,
                :producer,
                :aggregateId,
                :subjectKey,
                :sourceVersion,
                :occurredAt,
                :processedAt,
                :correlationId,
                :schemaVersion
            )
            ON CONFLICT (event_id) DO NOTHING
            """,
            parameters
        );

        return insertedRows == 1;

    }
    
}
