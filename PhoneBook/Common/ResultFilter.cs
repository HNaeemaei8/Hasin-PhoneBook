using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PhoneBook.Domain.Common;
using PhoneBook.Domain.Errors;
using PhoneBook.Application.Common;

namespace PhoneBook.Api.Common;

public class ResultFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executed = await next();

        if (executed.Exception != null || executed.Result is not ObjectResult objectResult)
            return;

        var value = objectResult.Value;
        if (value is null) return;

        var type = value.GetType();

        // Result<T>
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var isSuccess = (bool)type.GetProperty(nameof(Result.IsSuccess))!.GetValue(value)!;

            if (!isSuccess)
            {
                var errorCode = (DomainErrorCode)type.GetProperty(nameof(Result.ErrorCode))!.GetValue(value)!;
                executed.Result = MapErrorToResponse(errorCode);
                return;
            }

            var rawValue = type.GetProperty(nameof(Result<object>.Value))!.GetValue(value);
            executed.Result = new OkObjectResult(rawValue);
            return;
        }

        if (value is Result result)
        {
            executed.Result = result.IsSuccess
                ? new NoContentResult()
                : MapErrorToResponse(result.ErrorCode);
        }
    }

    private static IActionResult MapErrorToResponse(DomainErrorCode errorCode)
    {
        var message = errorCode.GetErrorMessage();

        return errorCode switch
        {
            DomainErrorCode.ContactNotFound => new NotFoundObjectResult(new { error = message }),
            DomainErrorCode.InvalidPhoneNumber => new BadRequestObjectResult(new { error = message }),
            _ => new BadRequestObjectResult(new { error = message })
        };
    }
}