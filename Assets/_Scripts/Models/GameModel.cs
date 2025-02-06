using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameModel {
    #region Variables
    public GamePhase GamePhase { get; private set; }
    [field: SerializeField] public List<PlayerModel> PlayerModels { get; private set; }
    public List<bool> Readys { get; private set; }
    public MapModel MapModel { get; private set; }
    public LootModel LootModel { get; private set; }
    #endregion


    public GameModel() {
        PlayerModels = new List<PlayerModel>();
        Readys = new List<bool>();
        GamePhase = new GamePhase();
        MapModel = new MapModel();
        LootModel = new LootModel();
    }


    public int CreatePlayer() {
        PlayerModels.Add(new PlayerModel(PlayerModels.Count));
        Readys.Add(false);

        MapModel.AddPlayer();

        return PlayerModels.Count - 1;
    }

    public void Ready(int playerID, bool value, out bool result) {
        Readys[playerID] = value;
        result = Readys.FindAll(b => b == false).Count == 0;
    }

    public void ClearReadys() {
        for (int i = 0; i < Readys.Count; i++)
            Readys[i] = false;
    }

    public void SetGamePhase(GamePhase gamePhase) {
        GamePhase = gamePhase;
    }


    public void Vote(int playerID, int value, out List<int> result) {
        if (GamePhase == GamePhase.Map) {
            MapModel.UpdateVotes(playerID, value, out result);
            return;
        }

        result = new List<int>();
        foreach (PlayerModel player in PlayerModels)
            result.Add(GameUtilsAndConsts.EMPTY_VOTE);
    }

    public void ClearVotes() {
        if (GamePhase == GamePhase.Map)
            MapModel.ClearVotes();
    }

    public VoteResult GetVoteResult() {
        if (GamePhase == GamePhase.Map)
            return MapModel.GetVoteResult();

        return new VoteResult(true, 0, 0);
    }


    public void LootTaken(int playerID, int lootIndex, int itemID) {
        LootModel.TakeLoot(lootIndex);
        PlayerModels[playerID].AddItemToInventory(itemID);
    }
}
