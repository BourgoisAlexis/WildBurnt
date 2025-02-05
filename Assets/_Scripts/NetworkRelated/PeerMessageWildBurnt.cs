public class PeerMessageWildBurnt : PeerMessage {
    public int SenderID;
    public bool IsBroadcasted;
    public MessageType MessageType;

    public PeerMessageWildBurnt(int senderID, bool isBroadcasted, MessageType messageType, string content) : base(content) {
        SenderID = senderID;
        IsBroadcasted = isBroadcasted;
        MessageType = messageType;
    }
}
