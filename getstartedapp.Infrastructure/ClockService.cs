using System;
using getstartedapp.Services;

namespace getstartedapp.Infrastructure;

public sealed class ClockService : IClockService
{
    public DateTime UtcNow => DateTime.UtcNow;
}
