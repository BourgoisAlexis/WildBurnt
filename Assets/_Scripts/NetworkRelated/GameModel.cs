using System;
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
    public List<bool> Readys { get; private set; }
    public GamePhase GamePhase { get; private set; }
    public List<TileData[]> TileRows { get; private set; }


    public GameModel() {
        PlayerModels = new List<PlayerModel>();
        CurrentVote = new List<int>();
        Readys = new List<bool>();
        GamePhase = new GamePhase();
        TileRows = new List<TileData[]>();
    }


    public int CreatePlayer() {
        PlayerModels.Add(new PlayerModel(PlayerModels.Count));
        CurrentVote.Add(GameUtilsAndConsts.EMPTY_VOTE);
        Readys.Add(false);
        return PlayerModels.Count - 1;
    }

    public void UpdateVote(int playerID, int value, out List<int> result) {
        CurrentVote[playerID] = value;
        result = CurrentVote;
    }

    public VoteResult GetVoteResult() {
        VoteResult result = new VoteResult(false, GameUtilsAndConsts.EMPTY_VOTE, 0);
        TileData[] row = TileRows.Last();

        int maxCount = CurrentVote.GroupBy(n => n).Max(g => g.Count());
        List<int> voteResults = CurrentVote.GroupBy(n => n).Where(g => g.Count() == maxCount).Select(g => g.Key).ToList();
        voteResults = voteResults.Select(n => n).Where(n => n != GameUtilsAndConsts.EMPTY_VOTE).ToList();

        if (voteResults.Count > 1) {
            voteResults.Shuffle();
            int index = voteResults.First();
            result = new VoteResult(true, index, (int)row[index].TileType);
        }
        else if (voteResults.Count <= 0) {
            int length = TileRows.Last().Length;
            int r = length > 1 ? UnityEngine.Random.Range(0, length) : 0;
            result = new VoteResult(length > 1, r, (int)row[r].TileType);
        }
        else {
            int index = voteResults.First();
            result = new VoteResult(false, index, (int)row[index].TileType);
        }

        return result;
    }

    public void ClearVotes() {
        for (int i = 0; i < CurrentVote.Count; i++)
            CurrentVote[i] = GameUtilsAndConsts.EMPTY_VOTE;
    }


    public void UpdateReady(int playerID, bool value, out List<bool> result) {
        Readys[playerID] = value;
        result = Readys;
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
