﻿namespace nScheduler.Domain.Models;

public interface IAggregateRoot<T>
{
    T Id { get; init; }
}

public class BaseAggregateRoot<T> : IAggregateRoot<T>
{
    public T Id { get; init; }

    public string OperName { get; init; }

    public DateTime OperTime { get; init; }
}