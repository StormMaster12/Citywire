using System;

namespace App.Abstraction;

public interface IDateTimeProvider
{
    DateTime Now();
}