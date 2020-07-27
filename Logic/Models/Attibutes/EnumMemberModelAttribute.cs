using System;

namespace EtherBetClientLib.Models
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EnumMemberModelAttribute : Attribute
    {
        public Type ModelType { get; }
        public EnumMemberModelAttribute(Type modelType)
        {
            ModelType = modelType;

        }
    }
}
