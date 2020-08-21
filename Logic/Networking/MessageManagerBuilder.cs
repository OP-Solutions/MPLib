using System;
using System.Collections.Generic;
using System.Text;
using MPLib.Models.Games;

namespace MPLib.Networking
{
    public class MessageManagerBuilder : IMessageConfigBuilder
    {
        private readonly PlayerIdentifyMode _identifyMode;
        private TypeCodeMapper _typeCodeMapper = TypeCodeMapper.Empty();

        public MessageManagerBuilder(PlayerIdentifyMode identifyMode)
        {
            _identifyMode = identifyMode;
        }


        public MessageManagerBuilder AddMessageTypesFromEnum<TEnumType>() 
        {
            _typeCodeMapper.AddEnum<TEnumType>();
            return this;
        }

        IMessageConfigBuilder IMessageConfigBuilder.AddMessageTypesFromEnum<TEnumType>()
        {
            return AddMessageTypesFromEnum<TEnumType>();
        }

        public IPlayerMessageManager Build(IReadOnlyList<Player> players)
        {
            return new PlayerMessageManager(players, _typeCodeMapper, _identifyMode);
        }
    }

    public interface IMessageConfigBuilder
    {
        public IMessageConfigBuilder AddMessageTypesFromEnum<TEnumType>();
    }
}
