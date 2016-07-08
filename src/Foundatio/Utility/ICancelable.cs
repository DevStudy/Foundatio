using System;

namespace Foundatio.Utility {
    interface ICancelable : IDisposable {
        bool IsDisposed { get; }
    }
}