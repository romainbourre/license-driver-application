using DriverLicenseApplication.Domain.Entities;
using DriverLicenseApplication.Domain.ValueObjects;


namespace DriverLicenseApplication.Application.Tests.Builders;

public class DriverLicenseBuilder
{
    private Guid licenseId = Guid.NewGuid();
    private DriverLicense.States state = DriverLicense.States.Current;

    public DriverLicenseBuilder WithId(Guid id)
    {
        licenseId = id;
        return this;
    }
    
    public DriverLicense Create()
    {
        return new DriverLicense(LicenseIdentifier.From(licenseId), state);
    }
    
    public static DriverLicenseBuilder GivenDriverLicense()
    {
        return new DriverLicenseBuilder();
    }
}
