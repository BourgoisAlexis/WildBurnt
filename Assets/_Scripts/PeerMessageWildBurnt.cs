public class PeerMessageWildBurnt : PeerMessage {
    public string Content;

    public PeerMessageWildBurnt(MessageType messageType, string content) : base(messageType.ToString()) {
        Content = content;
    }
}
