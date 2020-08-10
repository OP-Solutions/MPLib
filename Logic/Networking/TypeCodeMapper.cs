using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MPLib.Core.Game.Poker.Messaging;
using MPLib.Models.Attibutes;

namespace MPLib.Networking
{
    internal struct TypeCodeMapper
    {
        private Dictionary<int, Type> _codeToType;
        private Dictionary<Type, int> _typeToCode;
        private int _maxCodeValue;

        public static TypeCodeMapper FromEnum<T>() where T : Enum
        {
            var mapper = new TypeCodeMapper
            {
                _codeToType = new Dictionary<int, Type>(), 
                _typeToCode = new Dictionary<Type, int>()
            };
            mapper.AddEnum<T>();

            return mapper;
        }

        internal static TypeCodeMapper Empty()
        {
            var mapper = new TypeCodeMapper
            {
                _codeToType = new Dictionary<int, Type>(),
                _typeToCode = new Dictionary<Type, int>()
            };

            return mapper;
        }

        public void AddEnum<TEnum>()
        {
            var typeCodeOffset = _maxCodeValue + 1;
            var values = Enum.GetValues(typeof(TEnum));
            foreach (var value in values)
            {
                var memberName = value.ToString();
                var memberValue = (int)value;
                var typeCode = typeCodeOffset + (short)memberValue;
                var memberInfo = typeof(TEnum).GetMember(memberName)[0];
                var attribute = (EnumMemberModelAttribute)memberInfo.GetCustomAttributes(typeof(EnumMemberModelAttribute)).FirstOrDefault();
                if (attribute == null) continue;
                var type = attribute.ModelType;
                _codeToType[typeCode] = type;
                _typeToCode[type] = typeCode;
                if (typeCode > _maxCodeValue) _maxCodeValue = typeCode;
            }
        }

        public readonly Type GetType(int typeCode)
        {
            return _codeToType[typeCode];
        }

        public readonly int GetCode(Type type)
        {
            return _typeToCode[type];
        }
    }
}
