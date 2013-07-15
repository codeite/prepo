using System;

namespace prepo.Api.Infrastructure.Reflecting
{
    public class MemberAccessHelperException : Exception
    {
        public MemberAccessHelperException()
        {
        }

        public MemberAccessHelperException(string message) : base(message)
        {
        }

        public MemberAccessHelperException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}