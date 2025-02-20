public class PeerMessageWildBurnt : PeerMessage {
    public int SenderId;
    public bool IsBroadcasted;
    public MessageType MessageType;

    public PeerMessageWildBurnt(int senderID, bool isBroadcasted, MessageType messageType, string content) : base(content) {
        SenderId = senderID;
        IsBroadcasted = isBroadcasted;
        MessageType = messageType;
    }
}
