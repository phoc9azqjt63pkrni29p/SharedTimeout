using System;
using System.Threading;

namespace SharedTimeout
{
    public class SharedTimeout : IDisposable
    {
        public static SharedTimeout FromTimeSpan(TimeSpan value) => new SharedTimeout(value);
        public static SharedTimeout FromMilliseconds(int value) => new SharedTimeout(TimeSpan.FromMilliseconds(value));
        public static SharedTimeout FromSeconds(int value) => new SharedTimeout(TimeSpan.FromSeconds(value));
        public static SharedTimeout FromMinutes(int value) => new SharedTimeout(TimeSpan.FromMinutes(value));
        public static SharedTimeout FromHours(int value) => new SharedTimeout(TimeSpan.FromHours(value));
        public static SharedTimeout FromDays(int value) => new SharedTimeout(TimeSpan.FromDays(value));

        public static implicit operator TimeSpan(SharedTimeout timeout) => timeout.CreatedAt + timeout.Value - DateTime.Now;
        public static implicit operator int(SharedTimeout timeout) => (timeout.CreatedAt + timeout.Value - DateTime.Now).Seconds;
        public static implicit operator CancellationToken(SharedTimeout timeout)
        {
            if (timeout.Cts == null)
                timeout.Cts = new CancellationTokenSource(timeout.CreatedAt + timeout.Value - DateTime.Now);
            return timeout.Cts.Token;
        }

        private SharedTimeout(TimeSpan value)
        {
            CreatedAt = DateTime.Now;
            Value = value;
        }

        private SharedTimeout() : this(TimeSpan.Zero) { }

        private DateTime CreatedAt { get; }
        private TimeSpan Value { get; }
        private CancellationTokenSource Cts { get; set; }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Cts?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private bool disposedValue;
        #endregion
    }
}
