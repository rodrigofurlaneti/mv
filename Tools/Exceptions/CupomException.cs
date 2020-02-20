using System;

namespace Core.Exceptions
{
    public class CupomNotExistException : Exception
    {
        public CupomNotExistException(string message) : base(message) { }
    }

    public class CupomExpiredException : Exception
    {
        public CupomExpiredException(string message) : base(message) { }
    }
}