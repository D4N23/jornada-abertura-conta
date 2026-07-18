package com.journey.journey_resolution_read.entrypoint.api;

import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.journey.journey_resolution_read.application.query.ResolveJourneyResolutionQuery;
import com.journey.journey_resolution_read.entrypoint.api.dto.JourneyResolutionQuery;
import com.journey.journey_resolution_read.entrypoint.api.dto.JourneyResolutionResponse;

import jakarta.validation.Valid;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;


@RestController
@RequestMapping("/internal/v1/journey-resolutions")
public class JourneyResolutionController {

    private final ResolveJourneyResolutionQuery query;

    public JourneyResolutionController(ResolveJourneyResolutionQuery query) {
        this.query = query;
    }

    @PostMapping("/resolve")
    public ResponseEntity<JourneyResolutionResponse> resolve(
        @Valid @RequestBody JourneyResolutionQuery request
    ) {
       return query.execute(request.subjectKey())
              .map(result -> ResponseEntity.ok(
                new JourneyResolutionResponse(
                    result.nextAction(),
                    result.projectionVersion(),
                    result.updateAt()
                )
              )).orElse(
                ResponseEntity.notFound().build()
              );
    }    
}
