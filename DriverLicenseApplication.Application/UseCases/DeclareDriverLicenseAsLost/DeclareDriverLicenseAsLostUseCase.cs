using DriverLicenseApplication.Application.Extensions;
using DriverLicenseApplication.Application.Interfaces;
using DriverLicenseApplication.Domain.Entities;
using DriverLicenseApplication.Domain.ValueObjects;


namespace DriverLicenseApplication.Application.UseCases.DeclareDriverLicenseAsLost;

public class DeclareDriverLicenseAsLostUseCase : IDeclareDriverLicenseAsLost
{
    private readonly IDriverLicenseStorage driverLicenseStorage;

    public DeclareDriverLicenseAsLostUseCase(IDriverLicenseStorage driverLicenseStorage)
    {
        this.driverLicenseStorage = driverLicenseStorage;
    }

    public DeclareDriverLicenseAsLostResponse Handle(DeclareDriverLicenseAsLostRequest request)
    {
        DriverLicense driverLicense = GetDriverLicense(request.LicenseNumber);
        DriverLicense lostLicense = DeclareLicenseAsLost(driverLicense);
        SaveLostLicense(lostLicense);
        return new DeclareDriverLicenseAsLostResponse();
    }

    private DriverLicense GetDriverLicense(string licenseId)
    {
        LicenseIdentifier licenseIdentifier = LicenseIdentifier.From(licenseId);
        return driverLicenseStorage.GetDriverLicenseOrThrow(licenseIdentifier);
    }

    private static DriverLicense DeclareLicenseAsLost(DriverLicense driverLicense)
    {
        DriverLicense updatedDriverLicense = driverLicense with {State = DriverLicense.States.Lost};
        return updatedDriverLicense;
    }
    
    private void SaveLostLicense(DriverLicense updatedDriverLicense)
    {
        driverLicenseStorage.Update(updatedDriverLicense);
    }
}
