using System;
using App.Abstraction;

namespace App.Infrastructure;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now()
    {
        return DateTime.Now;
    }
}