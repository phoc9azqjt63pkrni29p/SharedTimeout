using System;
using System.Threading;

namespace SharedTimeout
{
    /// <summary>
    /// Represents reusable timeout that can be implicitly casted to TimeSpan int or CancellationToken
    /// </summary>
    public class SharedTimeout : IDisposable
    {
        /// <summary>
        /// Returns a <see cref="SharedTimeout"/> for specified <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="value">Time interval</param>
        /// <returns>An object that represents value.</returns>
        public static SharedTimeout FromTimeSpan(TimeSpan value) => new SharedTimeout(value);
        /// <summary>
        /// Returns a <see cref="SharedTimeout"/> for specified number of milliseconds.
        /// </summary>
        /// <param name="value">A number of milliseconds</param>
        /// <returns>An object that represents value.</returns>
        public static SharedTimeout FromMilliseconds(int value) => new SharedTimeout(TimeSpan.FromMilliseconds(value));
        /// <summary>
        /// Returns a <see cref="SharedTimeout"/> for specified number of seconds.
        /// </summary>
        /// <param name="value">A number of seconds</param>
        /// <returns>An object that represents value.</returns>
        public static SharedTimeout FromSeconds(int value) => new SharedTimeout(TimeSpan.FromSeconds(value));
        /// <summary>
        /// Returns a <see cref="SharedTimeout"/> for specified number of minutes.
        /// </summary>
        /// <param name="value">A number of minutes</param>
        /// <returns>An object that represents value.</returns>
        public static SharedTimeout FromMinutes(int value) => new SharedTimeout(TimeSpan.FromMinutes(value));
        /// <summary>
        /// Returns a <see cref="SharedTimeout"/> for specified number of hours.
        /// </summary>
        /// <param name="value">A number of hours</param>
        /// <returns>An object that represents value.</returns>
        public static SharedTimeout FromHours(int value) => new SharedTimeout(TimeSpan.FromHours(value));
        /// <summary>
        /// Returns a <see cref="SharedTimeout"/> for specified number of days.
        /// </summary>
        /// <param name="value">A number of days</param>
        /// <returns>An object that represents value.</returns>
        public static SharedTimeout FromDays(int value) => new SharedTimeout(TimeSpan.FromDays(value));
        /// <summary> Implicitly converts <paramref name="timeout"/> to remaining time interval </summary>
        public static implicit operator TimeSpan(SharedTimeout timeout) => timeout.Value;
        /// <summary> Implicitly converts <paramref name="timeout"/> to remaining number of seconds </summary>
        public static implicit operator int(SharedTimeout timeout) => (int)timeout.Value.TotalSeconds;
        /// <summary> Implicitly converts <paramref name="timeout"/> to <see cref="CancellationToken"/> which will be cancelled after remaining time interval </summary>
        public static implicit operator CancellationToken(SharedTimeout timeout)
        {
            if (timeout.Cts == null)
                timeout.Cts = new CancellationTokenSource(timeout.Value);
            return timeout.Cts.Token;
        }
        /// <summary> Explicit remaining time </summary>
        public TimeSpan Value => CreatedAt + _value - DateTime.Now;
        /// <summary> Explicit remaining time in milliseconds </summary>
        public int ValueMilliseconds => (int)Value.TotalMilliseconds;
        /// <summary> Explicit remaining time in seconds </summary>
        public int ValueSeconds => (int)Value.TotalSeconds;
        /// <summary> Explicit remaining time in minutes </summary>
        public int ValueMinutes => (int)Value.TotalMinutes;
        /// <summary> Explicit remaining time in hours </summary>
        public int ValueHours => (int)Value.TotalHours;
        /// <summary> Explicit remaining time in days </summary>
        public int ValueDays => (int)Value.TotalDays;

        private SharedTimeout(TimeSpan value)
        {
            CreatedAt = DateTime.Now;
            _value = value;
        }

        private SharedTimeout() : this(TimeSpan.Zero) { }

        private DateTime CreatedAt { get; }

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

        private readonly TimeSpan _value;
    }
}
