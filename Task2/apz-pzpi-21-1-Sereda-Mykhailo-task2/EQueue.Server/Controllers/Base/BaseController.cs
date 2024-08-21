using EQueue.Shared.ServiceResponseHandling;
using Microsoft.AspNetCore.Mvc;

namespace EQueue.Server.Controllers.Base
{
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult ConvertFromServiceResponse(ServiceResponse serviceResponse)
        {
            if (serviceResponse.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(serviceResponse.ErrorMessage);
        }

        protected IActionResult ConvertFromServiceResponse<T>(ServiceResponse<T> serviceResponse)
        {
            if (serviceResponse.IsSuccess)
            {
                return Ok(serviceResponse.Result);
            }
            return BadRequest(serviceResponse.ErrorMessage);
        }
    }
}
