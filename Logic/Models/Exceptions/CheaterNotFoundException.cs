using System;
using System.Runtime.Serialization;

namespace MPLib.Models.Exceptions
{
    [Serializable]
    public class CheaterNotFoundException : Exception
    {

        public CheaterNotFoundException() : base("No player cheated during shuffling process")
        {
        }
        public CheaterNotFoundException(string? message)
            : base(message)
        {
        }

        public CheaterNotFoundException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        protected CheaterNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}