using DriverLicenseApplication.Application.Exceptions;
using DriverLicenseApplication.Application.Tests.Adapters.Persistence;
using DriverLicenseApplication.Application.UseCases.DeclareDriverLicenseAsLost;
using DriverLicenseApplication.Domain.Entities;
using DriverLicenseApplication.Domain.Exceptions;
using FluentAssertions;
using static DriverLicenseApplication.Application.Tests.Builders.DriverLicenseBuilder;
using static DriverLicenseApplication.Application.Tests.Builders.DeclareDriverLicenseAsLostRequestBuilder;


namespace DriverLicenseApplication.Application.Tests.UseCases;

public class DeclareDriverLicenseAsLostTests
{
    private readonly InMemoryDriverLicenseStorage driverLicenseStorage = new();
    private readonly DeclareDriverLicenseAsLostUseCase useCase;

    protected DeclareDriverLicenseAsLostTests()
    {
        this.useCase = new DeclareDriverLicenseAsLostUseCase(this.driverLicenseStorage);
    }

    public class GivenDriverLicenseNumber : DeclareDriverLicenseAsLostTests
    {
        public class AndDriverLicenseExist : GivenDriverLicenseNumber
        {
            [Fact]
            public void ShouldInvalidLicenseWithLostState()
            {
                // Given
                var licenseId = Guid.NewGuid();
                DriverLicense license = GivenDriverLicense().WithId(licenseId).Create();
                driverLicenseStorage.HaveAlreadyDriverLicense(license);
                DeclareDriverLicenseAsLostRequest request = GivenLicenseLostRequest().WithId(licenseId.ToString()).Create();
                
                // Execute
                useCase.Handle(request);
                
                // Assert
                driverLicenseStorage.AssertThatContainLicense(license with
                {
                    State = DriverLicense.States.Lost,
                });
            }
        }

        public class ButDriverLicenseDoesntExist : GivenDriverLicenseNumber
        {
            [Fact]
            public void ShouldPreventThatLicenseDoesntExist()
            {
                // Given
                DeclareDriverLicenseAsLostRequest request = GivenLicenseLostRequest().Create();
                
                // Execute and Assert
                Action act = () => useCase.Handle(request);
                act.Should().Throw<ResourceNotFoundException>()
                    .WithMessage($"driver license with number {request.LicenseNumber} not found");
            }
        }
    }

    public class GivenNonWellFormattedLicenseNumber : DeclareDriverLicenseAsLostTests
    {
        [Theory]
        [InlineData("This is bad license number")]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        public void ShouldPreventValidationError(string licenceNumber)
        {
            // Given
            DeclareDriverLicenseAsLostRequest request = GivenLicenseLostRequest().WithId(licenceNumber).Create();

            // Execute and Assert
            Action act = () => useCase.Handle(request);
            act.Should().Throw<ValidationException>()
                .WithMessage(ValidationMessages.NonWellFormattedLicenseNumber);
        }
    }
}
