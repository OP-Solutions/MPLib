using System;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging
{
    [ProtoContract]
    public class Package<TBaseMessageType> where TBaseMessageType : IMessage
    {
        /// <summary>
        /// Sender identifier. It can be senders public key, sender player index index in game, etc..
        /// But it strictly depends on situation. for example if we are sending message in poker round it should be always sender player index in round, not anything else
        /// </summary>
        [ProtoMember(1)]
        public string SenderIdentifier { get; set; }

        /// <summary>
        /// Destination identifier, same rules as: <see cref="SenderIdentifier"/>
        /// </summary>
        [ProtoMember(2)]
        public string DestinationIdentifier { get; set; }

        [ProtoMember(3)]
        public DateTime TimeStampUtc { get; set; }

        [ProtoMember(4)]
        public TBaseMessageType Message { get; set; }

        public byte[] Signature { get; set; }
    }

    public interface IMessage
    {
    }
}
