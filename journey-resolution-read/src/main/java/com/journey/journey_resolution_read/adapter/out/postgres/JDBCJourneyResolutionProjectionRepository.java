package com.journey.journey_resolution_read.adapter.out.postgres;

import java.sql.ResultSet;
import java.sql.SQLException;
import java.time.Instant;
import java.time.OffsetDateTime;
import java.time.ZoneOffset;
import java.util.List;
import java.util.Objects;
import java.util.Optional;

import org.springframework.jdbc.core.RowMapper;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.namedparam.NamedParameterJdbcTemplate;

import com.journey.journey_resolution_read.model.CustomerStatus;
import com.journey.journey_resolution_read.model.IdentityStatus;
import com.journey.journey_resolution_read.model.JourneyResolutionProjection;
import com.journey.journey_resolution_read.model.NextAction;
import com.journey.journey_resolution_read.model.OnboardingStatus;
import com.journey.journey_resolution_read.port.out.JourneyResolutionProjectionRepository;

public class JDBCJourneyResolutionProjectionRepository implements JourneyResolutionProjectionRepository{

    private final NamedParameterJdbcTemplate template;

    public JDBCJourneyResolutionProjectionRepository(NamedParameterJdbcTemplate template) {
        this.template = template;
    }

    @Override
    public Optional<JourneyResolutionProjection> findBySubjectKey(String subjectKey) {
         var parameter = new MapSqlParameterSource()
                    .addValue("subjectKey", subjectKey);
        
        List<JourneyResolutionProjection> results = template
                .query("""
                    SELECT 
                        subject_key,
                        identity_status,
                        identity_version,
                        customer_status,
                        customer_version,
                        onboarding_status,
                        onboarding_version,
                        next_action,
                        projection_version,
                        updated_at
                    FROM journey_resolution
                    WHERE subject_key = :subjectKey
                    """
                    , parameter, rowMapper());

        return results.stream().findFirst();
    }

    @Override
    public JourneyResolutionProjection lockOrCreate(String sbjectKey, Instant now) {
       var parameters = new MapSqlParameterSource()
                .addValue(sbjectKey, now)
                .addValue("update_at", OffsetDateTime.ofInstant(now, ZoneOffset.UTC));

       template.update("""
            INSERT INTO journey_resolution (
                subject_key,
                identity_status,
                identity_version,
                customer_status,
                customer_version,
                onboarding_status,
                onboarding_version,
                next_action,
                projection_version,
                updated_at
            )
            VALUES (
                :subjectKey,
                'UNKNOWN',
                0,
                'UNKNOWN',
                0,
                'NONE',
                0,
                'NEW_ONBOARDING_ALLOWED',
                0,
                :updatedAt
            )
            ON CONFLICT (subject_key) DO NOTHING
            """, parameters);

            return Objects.requireNonNull(
                template.queryForObject(
                 """
                SELECT
                    subject_key,
                    identity_status,
                    identity_version,
                    customer_status,
                    customer_version,
                    onboarding_status,
                    onboarding_version,
                    next_action,
                    projection_version,
                    updated_at
                FROM journey_resolution
                WHERE subject_key = :subjectKey
                FOR UPDATE
                """,
                parameters,
                rowMapper())
            );
    }

    @Override
    public void save(JourneyResolutionProjection projection) {
        var parameters =  new MapSqlParameterSource()
        .addValue("subjectKey", projection.subjectKey())
        .addValue("identityStatus", projection.identityStatus().name())
        .addValue("identityVersion", projection.identityVersion())
        .addValue("customerStatus", projection.customerStatus().name())
        .addValue("customerVersion", projection.customerVersion())
        .addValue("onboardingStatus", projection.onboardingStatus().name())
        .addValue("onboardingVersion", projection.onboardingVersion())
        .addValue("nextAction", projection.nextAction())
        .addValue("projectionVersion", projection.projectionVersion())
        .addValue("updatedAt", OffsetDateTime.ofInstant(projection.updatedAt(), ZoneOffset.UTC));

        int rowsUpdated = template.update("""
                UPDATE journey_resolution
                SET
                    identity_status = :identityStatus,
                    identity_version = :identityVersion,
                    customer_status = :customerStatus,
                    customer_version = :customerVersion,
                    onboarding_status = :onboardingStatus,
                    onboarding_version = :onboardingVersion,
                    next_action = :nextAction,
                    projection_version = :projectionVersion,
                    updated_at = :updatedAt
                WHERE subject_key = :subjectKey
                """, parameters);
        
        if (rowsUpdated != 1) {
            throw new IllegalStateException("Journey resolution projection was not updated.");
        }
    }


    private RowMapper<JourneyResolutionProjection> rowMapper() {
        return(
            ResultSet resultSet,
            int rowNumber
        )-> mapProjection(resultSet);
    }

    private JourneyResolutionProjection mapProjection(ResultSet resultSet) throws SQLException {
        return new JourneyResolutionProjection(
            resultSet.getString("subject_key"),
            IdentityStatus.valueOf(resultSet.getString("identity_status")),
            resultSet.getLong("identity_version"),
            CustomerStatus.valueOf(resultSet.getString("customer_status")),
            resultSet.getLong("customer_version"),
            OnboardingStatus.valueOf(resultSet.getString("onboarding_status")),
            resultSet.getLong("onboarding_version"),
            NextAction.valueOf(resultSet.getString("next_action")),
            resultSet.getLong("projection_version"),
            resultSet.getObject(
                "update_at", OffsetDateTime.class
            ).toInstant()
        );
    }
    
}
