﻿using SinglePageArchitectureTraining.Frameworks.Abstracts;
using SinglePageArchitectureTraining.Frameworks.Contracts;

namespace SinglePageArchitectureTraining.Frameworks;

public class Response<T> : IResponse<T>
{
    public bool IsSuccessful { get; set; } = false;
    public Status Status { get; set; } = Status.Init;
    public string? Message { get; set; }
    public T? Value { get; set; }
}