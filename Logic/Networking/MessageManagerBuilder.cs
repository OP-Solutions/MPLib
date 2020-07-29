using System;
using System.Collections.Generic;
using System.Text;
using MPLib.Models.Games;

namespace MPLib.Networking
{
    public class MessageManagerBuilder
    {
        private readonly PlayerIdentifyMode _identifyMode;
        private TypeCodeMapper _typeCodeMapper = TypeCodeMapper.Empty();

        public MessageManagerBuilder(PlayerIdentifyMode identifyMode)
        {
            _identifyMode = identifyMode;
        }

        public MessageManagerBuilder UseMessageTypes<TEnumType>()
        {
            _typeCodeMapper.AddEnum<TEnumType>();
            return this;
        }

        public PlayerMessageManager<TBaseMessageType> Build<TBaseMessageType>(IReadOnlyList<Player> players) where TBaseMessageType : IMessage
        {
            return new PlayerMessageManager<TBaseMessageType>(players, _typeCodeMapper, _identifyMode);
        }
    }
}
