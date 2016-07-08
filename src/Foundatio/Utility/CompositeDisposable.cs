using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Foundatio.Utility {
    public sealed class CompositeDisposable : ICollection<IDisposable>, ICancelable {
        private readonly List<IDisposable> _items;
        private bool _disposed;

        public CompositeDisposable() {
            _items = new List<IDisposable>();
        }

        public CompositeDisposable(IEnumerable<IDisposable> disposables) {
            _items = new List<IDisposable>(disposables);
        }

        public CompositeDisposable(params IDisposable[] disposables) {
            if (disposables == null)
                throw new ArgumentNullException(nameof(disposables));
            if (disposables.Any(d => d == null))
                throw new ArgumentNullException(nameof(disposables), "Argument disposable parameter contains null");

            _items = new List<IDisposable>(disposables);
        }

        public CompositeDisposable(int capacity) {
            _items = new List<IDisposable>(capacity);
        }

        public int Count => _items.Count;

        public bool IsDisposed => _disposed;

        // FIXME: find out where this should be used as true.
        public bool IsReadOnly { get; internal set; }

        IEnumerator IEnumerable.GetEnumerator() {
            foreach (var i in _items)
                yield return i;
        }

        public void Add(IDisposable item) {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (_disposed)
                item.Dispose();
            else
                _items.Add(item);
        }

        public void Clear() {
            _items.Clear();
        }

        public bool Contains(IDisposable item) {
            return _items.Contains(item);
        }

        public void CopyTo(IDisposable[] array, int arrayIndex) {
            _items.CopyTo(array, arrayIndex);
        }

        public void Dispose() {
            if (_disposed)
                return;

            _disposed = true;
            foreach (var item in _items)
                item.Dispose();

            _items.Clear();
        }

        public IEnumerator<IDisposable> GetEnumerator() {
            foreach (var i in _items)
                yield return i;
        }

        public bool Remove(IDisposable item) {
            return _items.Remove(item);
        }
    }
}