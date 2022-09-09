using DriverLicenseApplication.Application.Exceptions;
using DriverLicenseApplication.Application.Interfaces;
using DriverLicenseApplication.Domain.Entities;
using DriverLicenseApplication.Domain.ValueObjects;


namespace DriverLicenseApplication.Application.Extensions;

internal static class DriverLicenseStorageExtension
{
    public static DriverLicense GetDriverLicenseOrThrow(this IDriverLicenseStorage driverLicenseStorage, LicenseIdentifier licenseIdentifier)
    {
        DriverLicense? license = driverLicenseStorage.GetByIdentifier(licenseIdentifier);
        if (license == null)
            throw new ResourceNotFoundException($"driver license with number {licenseIdentifier.Value} not found");
        return license;
    }
}
