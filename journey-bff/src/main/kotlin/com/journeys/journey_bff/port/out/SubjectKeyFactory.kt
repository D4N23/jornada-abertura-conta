package com.journeys.journey_bff.port.out

import com.journeys.journey_bff.application.model.SubjectKey

interface SubjectKeyFactory {
    fun fromCpf(rawCpf:String): SubjectKey
}