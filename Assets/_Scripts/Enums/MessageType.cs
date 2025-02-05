public enum MessageType {
    Default,

    //Connection
    AskForID,
    GiveID,

    //Game
    SetGamePhase,
    Ready,

    //Vote
    Vote,
    VoteTimer,
    VoteEnd,

    //Map
    MoveToTile,
    AddTileRow,
    BackToMap,

    //Loot
    AddLoots,
    TakeLoot,
}
