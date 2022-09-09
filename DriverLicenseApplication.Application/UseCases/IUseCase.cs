namespace DriverLicenseApplication.Application.UseCases;

public interface IUseCase<in TRequest, out TResponse>
{
    public TResponse Handle(TRequest request);
}
