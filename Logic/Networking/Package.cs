using System;
using ProtoBuf;

namespace MPLib.Networking
{
    [ProtoContract]
    public class Package
    {
        /// <summary>
        /// Sender identifier. It can be senders public key, sender player index index in game, etc..
        /// But it strictly depends on situation. for example if we are sending message in poker round it should be always sender player index in round, not anything else
        /// </summary>
        [ProtoMember(1)]
        public byte[] SenderIdentifier { get; set; }

        /// <summary>
        /// Destination identifier, same rules as: <see cref="SenderIdentifier"/>
        /// </summary>
        [ProtoMember(2)]
        public byte[] DestinationIdentifier { get; set; }

        [ProtoMember(3)]
        public DateTime TimeStampUtc { get; set; }

        #region These fields are [de]serialized manually

        public short MessageTypeCode { get; set; }

        public object Message { get; set; }

        public byte[] SenderSignature { get; set; } 
        #endregion
    }

    public interface IMessage
    {
    }
}
