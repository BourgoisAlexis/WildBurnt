public enum MessageType {
    Default,

    //Connection
    AskForId,
    GiveId,

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

    //Gear
    EquipGear,
    GearEquiped,
    UnequipGear,
    GearUnequiped,
}
