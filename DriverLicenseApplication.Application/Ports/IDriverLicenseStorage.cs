using DriverLicenseApplication.Domain.Entities;
using DriverLicenseApplication.Domain.ValueObjects;


namespace DriverLicenseApplication.Application.Interfaces;

public interface IDriverLicenseStorage
{

    DriverLicense? GetByIdentifier(LicenseIdentifier licenseIdentifier);
    void Update(DriverLicense updatedDriverLicense);
}
