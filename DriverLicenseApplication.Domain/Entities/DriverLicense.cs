using DriverLicenseApplication.Domain.ValueObjects;


namespace DriverLicenseApplication.Domain.Entities;

public record DriverLicense(LicenseIdentifier LicenseNumber, DriverLicense.States State)
{
    public enum States
    {
        Current,
        Lost,
    }
}
