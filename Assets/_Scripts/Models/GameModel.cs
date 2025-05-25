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
    public FightModel FightModel { get; private set; }
    #endregion


    public GameModel() {
        GamePhase = new GamePhase();
        PlayerModels = new List<PlayerModel>();
        Readys = new List<bool>();
        MapModel = new MapModel();
        LootModel = new LootModel();
        FightModel = new FightModel();
    }


    public void CreatePlayer(out int lastPlayerId) {
        PlayerModels.Add(new PlayerModel(PlayerModels.Count));
        Readys.Add(false);

        MapModel.CreatePlayer();

        lastPlayerId = PlayerModels.Count - 1;
    }


    public void Ready(int playerId, bool value, out bool result) {
        Readys[playerId] = value;
        result = Readys.FindAll(b => b == false).Count == 0;
    }

    public void ClearReadys() {
        for (int i = 0; i < Readys.Count; i++)
            Readys[i] = false;
    }


    public void Vote(int playerId, int value, out List<int> result) {
        if (GamePhase == GamePhase.Map) {
            MapModel.UpdateVotes(playerId, value, out result);
            return;
        }

        NetworkUtilsAndConsts.LogError("Trying to get votes out of GamePhase.Map");
        result = new List<int>();
        foreach (PlayerModel player in PlayerModels)
            result.Add(GameUtilsAndConsts.EMPTY_INT);
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


    public void SetGamePhase(GamePhase gamePhase) {
        GamePhase = gamePhase;
    }


    public void LootTaken(int playerId, int lootIndex, int itemId) {
        LootModel.RemoveLoot(lootIndex);
        PlayerModels[playerId].CharacterModel.AddItemToInventory(itemId);
    }


    public void GearEquiped(PlayerModel updatedModel) {
        PlayerModels[updatedModel.Id].CharacterModel.UpdateInventory(updatedModel.CharacterModel);
    }


    public void CreateFightBoard(CharacterModel[][] board) {
        FightModel.CreateBoard(board[0], board[1]);
    }
}
