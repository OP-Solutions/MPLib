using System;
using System.Runtime.Serialization;

namespace EtherBetClientLib.Models.Exceptions
{
    [Serializable]
    public class WrongSizeArrayException : Exception
    {

        public WrongSizeArrayException() : base("Array did not have right size")
        {
        }
        public WrongSizeArrayException(string? message)
            : base(message)
        {
        }

        public WrongSizeArrayException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        protected WrongSizeArrayException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}