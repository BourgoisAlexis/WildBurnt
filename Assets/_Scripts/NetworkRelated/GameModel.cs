using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct PlayerModel {
    public int ID;
    public string UserName;

    public PlayerModel(int id) {
        ID = id;
        UserName = $"Player_{id}";
    }
}


public class GameModel {
    public List<PlayerModel> PlayerModels { get; private set; }
    public List<int> CurrentVote { get; private set; }
    public GamePhase GamePhase { get; private set; }
    public List<TileData[]> TileRows { get; private set; }


    public GameModel() {
        PlayerModels = new List<PlayerModel>();
        CurrentVote = new List<int>();
        GamePhase = new GamePhase();
        TileRows = new List<TileData[]>();
    }


    public int CreatePlayer() {
        PlayerModels.Add(new PlayerModel(PlayerModels.Count));
        CurrentVote.Add(GameUtilsAndConsts.EMPTY_VOTE);
        return PlayerModels.Count - 1;
    }

    public List<int> UpdateVote(int playerID, int value) {
        CurrentVote[playerID] = value;
        return CurrentVote;
    }

    public List<int> GetVoteResults() {
        int maxCount = CurrentVote.GroupBy(n => n).Max(g => g.Count());

        return CurrentVote.GroupBy(n => n).Where(g => g.Count() == maxCount).Select(g => g.Key).ToList();
    }

    public void ClearVotes() {
        for (int i = 0; i < CurrentVote.Count; i++)
            CurrentVote[i] = GameUtilsAndConsts.EMPTY_VOTE;
    }

    public void AddTileRow(TileData[] tileDatas) {
        TileRows.Add(tileDatas);

        StringBuilder sb = new StringBuilder();
        foreach (TileData[] row in TileRows) {
            string s = string.Empty;
            for (int i = 0; i < row.Length; i++) {
                TileData tileData = row[i];
                s += tileData.TileType;
                if (i < row.Length - 1)
                    s += "-";
            }
            sb.AppendLine(s);
        }


        NetworkUtilsAndConsts.Log(sb.ToString());
    }

    public void UpdateGamePhase(GamePhase gamePhase) {
        GamePhase = gamePhase;
    }
}
