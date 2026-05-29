using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PhoneBook.Domain.Common;
using PhoneBook.Domain.Errors;
using PhoneBook.Application.Common;

namespace PhoneBook.Api.Common;

public class ResultFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(
        ResultExecutingContext context,
        ResultExecutionDelegate next)
    {
        if (context.Result is not ObjectResult objectResult)
        {
            await next();
            return;
        }

        if (objectResult.Value is Result result)
        {
            context.Result = result.IsSuccess
                ? new NoContentResult()
                : MapErrorToResponse(result.ErrorCode);

            return;
        }

        var value = objectResult.Value;
        if (value == null)
        {
            await next();
            return;
        }

        var type = value.GetType();
        if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Result<>))
        {
            await next();
            return;
        }

        var isSuccess = (bool)type.GetProperty(nameof(Result.IsSuccess))!.GetValue(value)!;

        if (!isSuccess)
        {
            var errorCode =
                (DomainErrorCode)type.GetProperty(nameof(Result.ErrorCode))!.GetValue(value)!;

            context.Result = MapErrorToResponse(errorCode);
            return;
        }

        var rawValue = type.GetProperty(nameof(Result<object>.Value))!.GetValue(value);
        context.Result = new OkObjectResult(rawValue);
    }

    private IActionResult MapErrorToResponse(DomainErrorCode errorCode)
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