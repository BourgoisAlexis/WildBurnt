public enum MessageType {
    Default,

    //Connection
    AskForId,
    GiveId,

    //Game
    SetGamePhase,
    Ready,
    EveryoneReady,

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

    //Fight
    UseAbility,
}
