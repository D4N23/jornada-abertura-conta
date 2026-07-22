using OnboardingService.Domain.Applications.ValueObjects;

namespace OnboardingService.Application.Abstractions;

public interface ISubjectKeyFactory
{
    SubjectKey CreateFrom(Cpf cpf);
}