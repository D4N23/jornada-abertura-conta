package com.journeys.journey_bff.adapter.out.security

import com.journeys.journey_bff.config.SubjectKeyProperties
import com.journeys.journey_bff.port.out.SubjectKeyFactory
import com.journeys.journey_bff.application.model.SubjectKey
import com.journeys.journey_bff.error.InvalidCpfException

import org.springframework.stereotype.Component
import  java.nio.charset.StandardCharsets
import javax.crypto.Mac
import javax.crypto.spec.SecretKeySpec
import java.util.Base64

@Component
class HmacSha256SubjectKeyFactory(private val properties: SubjectKeyProperties) : SubjectKeyFactory{
    
    override fun fromCpf(rawCpf:String): SubjectKey {
        val normalizedCpf = rawCpf.filter(Char::isDigit)
        
        if (!isValidCpf(normalizedCpf)){
            throw InvalidCpfException()
        }
        
        val mac = Mac.getInstance(HMAC_ALGORITHM)
        
        val key = SecretKeySpec(
            properties.secret.toByteArray(StandardCharsets.UTF_8),
            HMAC_ALGORITHM
        )
        
        mac.init(key)
        
        val fingerprint = mac.doFinal(
            normalizedCpf.toByteArray(StandardCharsets.UTF_8)
        )
        
        return SubjectKey(
            Base64.getUrlEncoder()
                .withoutPadding()
                .encodeToString(fingerprint)
        )
        
    }
    
    private fun isValidCpf(cpf: String): Boolean {
        if (cpf.length != CPF_LENGTH){
            return false
        }
        
        if (cpf.all{it == cpf.first()}){
            return false
        }
        
        val firstCheckDigit = calculateCheckDigit(
            digitis = cpf,
            length = 9        
        )
        
        if (cpf[9].digitToInt() != firstCheckDigit){
            return false
        }
        
        val secondCheckDigit = calculateCheckDigit(
            digitis = cpf,
            length = 10
        )
        
        return cpf[10].digitToInt() == secondCheckDigit
    }
    
    private fun calculateCheckDigit(
        digitis: String,
        length: Int
    ): Int {
      var sum = 0
      var weigtht = length + 1
      
      for (index in 0 until length){
        sum += digitis[index].digitToInt() * weigtht
        weigtht --
      }  
      
      val remainder = (sum * 10) % 11
      
      return  if (remainder == 10){
        0
      }else{
        remainder
      }
    }

    
    companion object{
        private const val CPF_LENGTH = 11
        private const val HMAC_ALGORITHM = "HmacSHA256"
    }
}
