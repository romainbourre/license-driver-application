using DriverLicenseApplication.Adapters.Controllers;
using DriverLicenseApplication.Adapters.Tests.Spies;
using DriverLicenseApplication.Application.Exceptions;
using DriverLicenseApplication.Application.UseCases.DeclareDriverLicenseAsLost;
using DriverLicenseApplication.Domain.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace DriverLicenseApplication.Adapters.Tests.Controllers;



public class DriverLicenseControllerTests
{
    private readonly LoggerSpy<DriverLicenseController> loggerSpy = new();
    private readonly DriverLicenseController controller;

    public DriverLicenseControllerTests()
    {
        this.controller = new DriverLicenseController(this.loggerSpy);
    }

    [Fact]
    public void Given_GoodRequest_When_DeclareDriverLicenseAsLost_Then_ReturnOkResponse()
    {
        var useCase = Mock.Of<IDeclareDriverLicenseAsLost>();
        var expectedResponse = new DeclareDriverLicenseAsLostResponse();
        Mock.Get(useCase).Setup(u => u.Handle(It.IsAny<DeclareDriverLicenseAsLostRequest>())).Returns(expectedResponse);
        IActionResult response = controller.DeclareDriverLicenseAsLost(useCase, "good parameter");
        response.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expectedResponse);
    }
    
    [Fact]
    public void Given_BadUserInput_When_DeclareDriverLicenseAsLost_Then_LogAndReturnBadRequest()
    {
        var useCase = Mock.Of<IDeclareDriverLicenseAsLost>();
        const string errorMessage = "a validation error";
        Mock.Get(useCase).Setup(u => u.Handle(It.IsAny<DeclareDriverLicenseAsLostRequest>())).Throws(() => new ValidationException(errorMessage));
        IActionResult response = controller.DeclareDriverLicenseAsLost(useCase, "any bad parameter");
        loggerSpy.AssertThatHaveLogged(LogLevel.Trace, $"{nameof(IDeclareDriverLicenseAsLost)}: {errorMessage}", typeof(ValidationException));
        response.Should().BeOfType<BadRequestResult>();
    }
    
    [Fact]
    public void Given_NonExistentDriverLicense_When_DeclareDriverLicenseAsLost_Then_ReturnNotFound()
    {
        var useCase = Mock.Of<IDeclareDriverLicenseAsLost>();
        const string errorMessage = "resource not found";
        Mock.Get(useCase).Setup(u => u.Handle(It.IsAny<DeclareDriverLicenseAsLostRequest>())).Throws(() => new ResourceNotFoundException(errorMessage));
        IActionResult response = controller.DeclareDriverLicenseAsLost(useCase, "an unknown id");
        loggerSpy.AssertThatHaveLogged(LogLevel.Warning, $"{nameof(IDeclareDriverLicenseAsLost)}: {errorMessage}", typeof(ResourceNotFoundException));
        response.Should().BeOfType<NotFoundResult>();
    }
    
    [Fact]
    public void Given_UnknownError_When_DeclareDriverLicenseAsLost_Then_ReturnInternalServerError()
    {
        var useCase = Mock.Of<IDeclareDriverLicenseAsLost>();
        const string errorMessage = "unknown exception";
        Mock.Get(useCase).Setup(u => u.Handle(It.IsAny<DeclareDriverLicenseAsLostRequest>())).Throws(() => new Exception(errorMessage));
        IActionResult response = controller.DeclareDriverLicenseAsLost(useCase, "a good id");
        loggerSpy.AssertThatHaveLogged(LogLevel.Critical, $"{nameof(IDeclareDriverLicenseAsLost)}: {errorMessage}",typeof(Exception));
        response.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}
