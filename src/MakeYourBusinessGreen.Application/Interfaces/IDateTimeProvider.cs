﻿namespace MakeYourBusinessGreen.Application.Interfaces;
public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}
