public enum MessageType {
    Default = 0,

    //Connection
    AskForID = 1,
    GiveID = 2,

    //Game
    UpdateGamePhase = 3,
    Ready = 4,

    //Vote
    Vote = 5,
    VoteTimer = 6,
    VoteEnd = 7,

    //Map
    MoveToTile = 8,
    AddTileRow = 9,
    BackToMap = 10,

    //Loot
    AddLoots = 11,
}
