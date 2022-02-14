using System;

namespace App.Abstraction;

public interface ICustomerValidator
{
    public bool ValidateCustomer(string firstname, string surname, string email, DateTime dateOfBirth);
}