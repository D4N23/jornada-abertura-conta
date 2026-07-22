using OnboardingService.Application.Abstractions;
using OnboardingService.Domain.Applications;
using OnboardingService.Domain.Applications.ValueObjects;

namespace OnboardingService.Application.Applications.StartApplication;

public sealed class StartApplicationHandler
{
    private readonly IAccountOpeningApplicationRepository _repository;

    private readonly ISubjectKeyFactory _subjectKeyFactory;

    private readonly IUnitOfWork _unitOfWork;

    private readonly TimeProvider _timeProvider;

    public StartApplicationHandler(
        IAccountOpeningApplicationRepository repository,
        ISubjectKeyFactory subjectKeyFactory,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider
    )
    {
        _repository = repository;
        _subjectKeyFactory = subjectKeyFactory;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
    }

    public async Task<StartApplicationResult> HandleAsync(
        StartApplicationCommand command,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(command);
        var cpf = Cpf.From(command.Cpf);
        var correlationId = CorrelationId.From(command.CorrelationId);
        var subjectKey = _subjectKeyFactory.CreateFrom(cpf);
        var activeApplication = await _repository.FindActiveBySubjectKeyAsync(subjectKey, cancellationToken);

        if(activeApplication is not null)
        {
            return StartApplicationResult.From(activeApplication, created: false);
        }

        var application = AccountOpeningApplication.Start(
            applicantCpf: cpf,
            subjectKey: subjectKey,
            correlationId: correlationId,
            now: _timeProvider.GetUtcNow()
        );

        await _repository.AddAsync(application, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return StartApplicationResult.From(application, created: true);
    }

}