using System;
using System.Collections.Generic;
using System.Text;

namespace Orleans.Storage.Application.Grains.Base;

public class BaseGrain : Grain
{
    private IGrainBase? _grainBase;

    /// <summary>
    /// Use this property to access GrainBase functionality from derived grains. It will reference the current grain instance by default, but can be set to another IGrainBase implementation if needed (e.g., for testing or composition purposes).
    /// </summary>
    protected IGrainBase GrainBaseReference { get => _grainBase ?? this; set => _grainBase = value; }
}
