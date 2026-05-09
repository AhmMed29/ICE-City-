using System;

namespace IceCity
{
    public class HeaterFailedException : Exception
    {
        public HeaterFailedException() : base("The heater has failed and needs replacement.")
        {
        }

        public HeaterFailedException(string message) : base(message)
        {
        }

        public HeaterFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}