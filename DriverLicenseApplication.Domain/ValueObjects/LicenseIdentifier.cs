using DriverLicenseApplication.Domain.Exceptions;


namespace DriverLicenseApplication.Domain.ValueObjects;

public readonly record struct LicenseIdentifier
{
    public readonly string Value;

    private LicenseIdentifier(string value)
    {
        if (!Guid.TryParse(value, out Guid parsedId) || parsedId == Guid.Empty)
            throw new ValidationException(ValidationMessages.NonWellFormattedLicenseNumber);
        Value = value;
    }

    public static LicenseIdentifier From(Guid licenseId)
    {
        return new LicenseIdentifier(licenseId.ToString());
    }

    public static LicenseIdentifier From(string licenseId)
    {
        return new LicenseIdentifier(licenseId);
    }
}
