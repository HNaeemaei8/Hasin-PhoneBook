namespace PhoneBook.Application.Common;
using PhoneBook.Domain.Errors;

public static class ErrorMessages
{
    private static readonly Dictionary<DomainErrorCode, string> _messages = new()
    {
        { DomainErrorCode.InvalidPhoneNumber, "شماره تلفن وارد شده معتبر نیست." },
        { DomainErrorCode.NameIsRequired, "نام مخاطب الزامی است." },
        { DomainErrorCode.LastNameIsRequired, "نام خانوادگی الزامی است." },
        { DomainErrorCode.ContactNotFound, "مخاطب مورد نظر یافت نشد." },
        { DomainErrorCode.ServerError, "خطای داخلی سرور رخ داده است." }
    };

    public static string GetMessage(DomainErrorCode code)
    {
        return _messages.TryGetValue(code, out var message) ? message : "خطای ناشناخته رخ داده است.";
    }
}