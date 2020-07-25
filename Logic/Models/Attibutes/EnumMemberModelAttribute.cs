using System;

namespace EtherBetClientLib.Models
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EnumMemberModelAttribute : Attribute
    {
        public EnumMemberModelAttribute(Type modelType)
        {

        }
    }
}
