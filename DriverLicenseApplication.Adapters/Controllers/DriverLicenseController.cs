using DriverLicenseApplication.Application.Exceptions;
using DriverLicenseApplication.Application.UseCases.DeclareDriverLicenseAsLost;
using DriverLicenseApplication.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace DriverLicenseApplication.Adapters.Controllers;

public class DriverLicenseController : ControllerBase
{
    private readonly ILogger<DriverLicenseController> logger;
    
    public DriverLicenseController(ILogger<DriverLicenseController> logger)
    {
        this.logger = logger;
    }

    [HttpPut("{licenseNumber}/as-lost")]
    public IActionResult DeclareDriverLicenseAsLost
    (
        [FromServices] IDeclareDriverLicenseAsLost useCase,
        string licenseNumber
    )
    {
        try
        {
            var request = new DeclareDriverLicenseAsLostRequest(licenseNumber);
            DeclareDriverLicenseAsLostResponse response = useCase.Handle(request);
            return Ok(response);
        }
        catch (ValidationException e)
        {
            logger.LogTrace(e, "{UseCaseName}: {ErrorMessage}", nameof(IDeclareDriverLicenseAsLost), e.Message);
            return BadRequest();
        }
        catch (ResourceNotFoundException e)
        {
            logger.LogWarning(e, "{UseCaseName}: {ErrorMessage}", nameof(IDeclareDriverLicenseAsLost), e.Message);
            return NotFound();
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "{UseCaseName}: {ErrorMessage}", nameof(IDeclareDriverLicenseAsLost), e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
