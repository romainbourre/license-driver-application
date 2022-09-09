using DriverLicenseApplication.Application.UseCases.DeclareDriverLicenseAsLost;


namespace DriverLicenseApplication.Application.Tests.Builders;

public class DeclareDriverLicenseAsLostRequestBuilder
{
    private string licenseId = Guid.NewGuid().ToString();

    public DeclareDriverLicenseAsLostRequestBuilder WithId(string id)
    {
        licenseId = id;
        return this;
    }
    
    public DeclareDriverLicenseAsLostRequest Create()
    {
        return new DeclareDriverLicenseAsLostRequest(LicenseNumber:licenseId);
    }

    public static DeclareDriverLicenseAsLostRequestBuilder GivenLicenseLostRequest()
    {
        return new DeclareDriverLicenseAsLostRequestBuilder();
    }
}
