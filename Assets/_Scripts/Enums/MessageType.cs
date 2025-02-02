public enum MessageType {
    Default,

    //Connection
    AskForID,
    GiveID,

    //Game
    UpdateGamePhase,
    Ready,

    //Vote
    Vote,
    VoteTimer,
    VoteEnd,

    //Map
    MoveToDestination,
    AddTileRow,
}
