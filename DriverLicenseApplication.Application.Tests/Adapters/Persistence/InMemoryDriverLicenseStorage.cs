using DriverLicenseApplication.Application.Interfaces;
using DriverLicenseApplication.Domain.Entities;
using DriverLicenseApplication.Domain.ValueObjects;
using FluentAssertions;


namespace DriverLicenseApplication.Application.Tests.Adapters.Persistence;

public class InMemoryDriverLicenseStorage : IDriverLicenseStorage
{
    public readonly List<DriverLicense> DriverLicenses = new();

    public void HaveAlreadyDriverLicense(DriverLicense license)
    {
        DriverLicenses.Add(license);
    }

    public void AssertThatContainLicense(DriverLicense license)
    {
        DriverLicenses.Should().Contain(license);
    }
    
    public DriverLicense? GetByIdentifier(LicenseIdentifier licenseIdentifier)
    {
        return DriverLicenses.FirstOrDefault(license => license.LicenseNumber == licenseIdentifier);
    }
    
    public void Update(DriverLicense updatedDriverLicense)
    {
        int index = DriverLicenses.FindIndex(license => license.LicenseNumber == updatedDriverLicense.LicenseNumber);
        DriverLicenses.RemoveAt(index);
        DriverLicenses.Add(updatedDriverLicense);
    }
}
