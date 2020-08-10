using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace MPLib.Models.Exceptions
{
    class PlayerRuleViolationException : Exception
    {
        public int GameCode { get; }
        public int ErrorCode { get; }
        public override string Message { get; }

        public PlayerRuleViolationException(int gameCode, int errorCode, string message)
        {
            GameCode = gameCode;
            ErrorCode = errorCode;
            Message = message;
        }
    }

    class PlayerRuleViolationException<TErrorCode> : PlayerRuleViolationException where TErrorCode : Enum
    {

        public new TErrorCode ErrorCode { get; }

        public PlayerRuleViolationException(int gameCode, TErrorCode errorCode) : base(gameCode, (int)(object)errorCode, GetErrorMessage(errorCode))
        {
            ErrorCode = errorCode;
        }

        private static string GetErrorMessage(TErrorCode errorCode)
        {
            var errorName = errorCode.ToString();

            var memberInfo = typeof(TErrorCode).GetMember(errorName)[0];

            var descriptionAttribute = memberInfo.GetCustomAttribute<DescriptionAttribute>();


            return descriptionAttribute.Description;
        }
    }
}
