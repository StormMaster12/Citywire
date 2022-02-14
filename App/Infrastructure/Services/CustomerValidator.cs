using System;
using App.Abstraction;

namespace App.Infrastructure.Services;

public class CustomerValidator : ICustomerValidator
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public CustomerValidator(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public bool ValidateCustomer(string firstname, string surname, string email, DateTime dateOfBirth)
    {
        if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(surname)) return false;

        if (!email.Contains("@") || !email.Contains(".")) return false;

        var now = _dateTimeProvider.Now();
        var age = now.Year - dateOfBirth.Year;
        if (now.Month < dateOfBirth.Month || now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day) age--;

        return age >= 21;
    }
}