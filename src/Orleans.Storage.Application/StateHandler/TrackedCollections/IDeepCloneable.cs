using System;
using System.Collections.Generic;
using System.Text;

namespace Orleans.Storage.Application.StateHandler.TrackedCollections;

public interface IDeepCloneable<T>
{
    T DeepClone();
}
