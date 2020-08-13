using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using MPLib.Core.Game;

namespace MPLib.Models.Exceptions
{
    class PlayerRuleViolationException : Exception
    {
        public Game Game { get; }
        public int ErrorCode { get; }
        public override string Message { get; }

        public PlayerRuleViolationException(Game game, int errorCode, string message)
        {
            Game = game;
            ErrorCode = errorCode;
            Message = message;
        }
    }

    class PlayerRuleViolationException<TErrorCode> : PlayerRuleViolationException where TErrorCode : Enum
    {

        public new TErrorCode ErrorCode { get; }

        public PlayerRuleViolationException(Game game, TErrorCode errorCode) : base(game, (int)(object)errorCode, GetErrorMessage(errorCode))
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
