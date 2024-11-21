using Azure.Core;
using Backend.Api.Filter;
using Backend.Application.Core;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Api.Controllers
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected ActionResult HandleResult (object data)
        {
            if (data == null)
            {
                return NotFound();
            }

            return Ok(new ResponseResult(data, 200,"Suceesfull",true) );
        }
        protected ActionResult HandleBadRequest(ValidationResult validationResult)
        {

            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            var response = new
            {
                errors = errors,
                type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                title = "One or more validation errors occurred.",
                status = 400,
                traceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return BadRequest(response);

        }
        protected IActionResult HandleResultGeneric<T>(Result<T> result)
        {
            if (result == null) return NotFound();

            if (result.IsSuccess && result.Value != null)
                return Ok(result.Value);

            if (result.IsSuccess && result.Value == null)
                return NotFound();

            return BadRequest(result.Error);
        }

    }

    // and pass it as below:
    //var data = _productPayloadHandler.GetLoadedPayload();
    //var responsedData = Result<ProductDTO>.Success(data);
    //return HandleResultGeneric(responsedData);



 
    


}
