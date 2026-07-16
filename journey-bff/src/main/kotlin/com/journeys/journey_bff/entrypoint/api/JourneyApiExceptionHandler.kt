package com.journeys.journey_bff.entrypoint.api

import com.journeys.journey_bff.error.InvalidCpfException
import com.journeys.journey_bff.error.JourneyResolutionUnavailableException

import org.springframework.web.bind.annotation.RestControllerAdvice
import org.springframework.web.bind.annotation.ExceptionHandler
import org.springframework.http.ProblemDetail
import org.springframework.http.HttpStatus


@RestControllerAdvice
class JourneyApiExceptionHandler{

    @ExceptionHandler(InvalidCpfException::class)
    fun handleInvalidCpf() : ProblemDetail{
        return ProblemDetail
                .forStatusAndDetail(HttpStatus.BAD_REQUEST, "The supplied CPF is invalid.")
                .apply{
                    title = "Invalid identifier"
                    setProperty(
                        "errorCode",
                        "INVALID_CPF"
                    )
                }
    } 

    @ExceptionHandler(JourneyResolutionUnavailableException::class)
    fun handlerJourneyResolutionUnavailable() : ProblemDetail{
        return ProblemDetail
                .forStatusAndDetail(HttpStatus.SERVICE_UNAVAILABLE, "The journey resolution service is temporarity unavailable.")
                .apply{
                    title = "Journey resolution unavailable"
                    setProperty(
                        "errorCode",
                        "JOURNEY_RESOLUTION_UNAVAIBLE"
                    )
                }
    } 

}