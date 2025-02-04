using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameModel {
    #region Variables
    public GamePhase GamePhase { get; private set; }
    public List<PlayerModel> PlayerModels { get; private set; }
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

    public void UpdateReadys(int playerID, bool value, out bool result) {
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


    public void UpdateVotes(int playerID, int value, out List<int> result) {
        if (GamePhase == GamePhase.Map) {
            MapModel.UpdateVotes(playerID, value, out result);
            return;
        }

        result = new List<int>();
        foreach (PlayerModel player in PlayerModels)
            result.Add(GameUtilsAndConsts.EMPTY_VOTE);
    }

    public VoteResult GetVoteResult() {
        if (GamePhase == GamePhase.Map)
            return MapModel.GetVoteResult();

        return new VoteResult(true, 0, 0);
    }

    public void ClearVotes() {
        if (GamePhase == GamePhase.Map)
            MapModel.ClearVotes();
    }
}
