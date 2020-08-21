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

        public MessageManagerBuilder WithMessageTypes<TEnumType>()
        {
            _typeCodeMapper.AddEnum<TEnumType>();
            return this;
        }

        public IPlayerMessageManager Build(IReadOnlyList<Player> players)
        {
            return new PlayerMessageManager(players, _typeCodeMapper, _identifyMode);
        }
    }
}
