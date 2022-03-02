using System;

namespace Atesh.BindableProperties;

// ReSharper disable once UnusedTypeParameter
public partial class PrivatelyBindableProperty<T>
{
    // Multiple constructors can run at the same time on different threads so we have to store the temp delegates in the thread.
    [ThreadStatic] static SetDelegates TempSetDelegates;
}