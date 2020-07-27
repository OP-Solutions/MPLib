using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using EtherBetClientLib.Models;

namespace EtherBetClientLib.Networking
{
    public struct TypeCodeMapper
    {
        private readonly Dictionary<short, Type> _codeToType;
        private Dictionary<Type, short> _typeToCode;

        public static TypeCodeMapper FromEnum<T>() where T : Enum
        {
            return new TypeCodeMapper(typeof(T));
        }

        private TypeCodeMapper(Type sourceEnum)
        {
            _codeToType = new Dictionary<short, Type>();
            _typeToCode = new Dictionary<Type, short>();
            var values = Enum.GetValues(sourceEnum);
            foreach (var value in values)
            {
                var memberName = value.ToString();
                var memberValue = (int)value;
                var typeCode = (short)memberValue;
                var memberInfo = sourceEnum.GetMember(memberName)[0];
                var attribute = (EnumMemberModelAttribute)memberInfo.GetCustomAttributes(typeof(EnumMemberModelAttribute)).FirstOrDefault();
                if(attribute == null) continue;
                var type = attribute.ModelType;
                _codeToType[typeCode] = type;
                _typeToCode[type] = typeCode;
            }
        }

        public readonly Type GetType(short typeCode)
        {
            return _codeToType[typeCode];
        }

        public readonly short GetCode(Type type)
        {
            return _typeToCode[type];
        }
    }
}
