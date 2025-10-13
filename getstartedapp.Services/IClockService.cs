using System;

namespace getstartedapp.Services;

public interface IClockService
{
    DateTime UtcNow { get; }
}