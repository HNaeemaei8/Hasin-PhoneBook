using PhoneBook.Domain.Common;
using PhoneBook.Domain.Errors;

namespace PhoneBook.Application.Common;

public static class ResultExtensions
{
    public static string GetErrorMessage(this Result result)
    {
        return ErrorMessages.GetMessage(result.ErrorCode);
    }

    public static string GetErrorMessage(this DomainErrorCode errorCode)
    {
        return ErrorMessages.GetMessage(errorCode);
    }
}
