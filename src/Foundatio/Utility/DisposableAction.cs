using System;
using System.Threading;

namespace Foundatio.Utility {
    public sealed class DisposableAction : IDisposable, ICancelable {
        private Action _disposeAction;

        public DisposableAction(Action disposeAction) {
            _disposeAction = disposeAction;
        }

        void IDisposable.Dispose() {
            var dispose = Interlocked.Exchange(ref _disposeAction, null);
            dispose?.Invoke();
        }

        public bool IsDisposed => _disposeAction == null;
    }
}