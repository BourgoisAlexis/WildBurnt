public enum MessageType {
    Default,

    //Connection
    AskForID,
    GiveID,

    //Game
    UpdateGamePhase,

    //Vote
    Vote,
    VoteTimer,
    VoteEnd,

    //Map
    MoveToDestination,
    AddTileRow,
}
