using System;

namespace Infrastructure.Caching
{
    public class CacheResult
    {
        public CacheResult(object result)
        {
            this.Result = result;

        }

        public CacheResult(Exception exception)
        {
            Exception = exception;
        }

        public object Result { get; private set; }
        public bool Error { get { return Exception != null; } }
        public Exception Exception { get; private set; }
    }
}