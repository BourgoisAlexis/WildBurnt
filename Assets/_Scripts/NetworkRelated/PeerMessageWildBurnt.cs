public class PeerMessageWildBurnt : PeerMessage {
    public int SenderID;
    public MessageType MessageType;

    public PeerMessageWildBurnt(int senderID, MessageType messageType, string content) : base(content) {
        MessageType = messageType;
        SenderID = senderID;
    }
}
