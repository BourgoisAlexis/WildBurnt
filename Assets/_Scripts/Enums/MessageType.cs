public enum MessageType {
    Default,

    //Connection
    AskForID,
    GiveID,

    //Game
    SetGamePhase,
    Ready,
    EveryOneReady,

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
    LootTaken,
}
