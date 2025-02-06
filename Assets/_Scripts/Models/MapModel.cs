using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MapModel {
    public List<TileModel[]> TileRows { get; private set; }
    public int CurrentTileIndex { get; private set; }
    public List<int> CurrentVote { get; private set; }
    public WeightedGenerator WeightedGenerator { get; private set; }


    public MapModel() {
        TileRows = new List<TileModel[]>();
        CurrentTileIndex = 0;
        CurrentVote = new List<int>();
        WeightedGenerator = new WeightedGenerator(Enum.GetNames(typeof(TileType)).Length, 1);
    }


    public TileModel[] CreateTileRow() {
        int rowSize = UnityEngine.Random.Range(1, 5);
        int[] array = WeightedGenerator.GenerateArray(rowSize);
        TileModel[] result = new TileModel[rowSize];

        for (int i = 0; i < rowSize; i++)
            result[i] = new TileModel(TileType.Loot);
        //result[i] = new TileModel((TileType)array[i]);

        NetworkUtilsAndConsts.Log($"-Generating only loots atm-");

        return result;
    }

    public void AddTileRow(TileModel[] tileDatas) {
        TileRows.Add(tileDatas);

        StringBuilder sb = new StringBuilder();
        foreach (TileModel[] row in TileRows) {
            string s = string.Empty;
            for (int i = 0; i < row.Length; i++) {
                TileModel tileModel = row[i];
                s += tileModel.TileType;
                if (i < row.Length - 1)
                    s += "-";
            }
            sb.AppendLine(s);
        }

        NetworkUtilsAndConsts.Log(sb.ToString());
    }

    public void MoveToTile(VoteResult result) {
        CurrentTileIndex = result.Index;
    }

    public TileModel GetCurrentTile() {
        return TileRows.Last()[CurrentTileIndex];
    }


    //Votes
    public void AddPlayer() {
        CurrentVote.Add(GameUtilsAndConsts.EMPTY_VOTE);
    }

    public void UpdateVotes(int playerID, int value, out List<int> result) {
        CurrentVote[playerID] = value;
        result = CurrentVote;
    }

    public VoteResult GetVoteResult() {
        VoteResult result = new VoteResult(false, GameUtilsAndConsts.EMPTY_VOTE, 0);
        TileModel[] row = TileRows.Last();

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
}
