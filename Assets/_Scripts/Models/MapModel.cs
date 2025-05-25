using System;
using System.Collections.Generic;
using System.Linq;


public class MapModel {
    #region Variables
    public List<TileModel[]> TileRows { get; private set; }
    public int CurrentTileIndex { get; private set; }
    public List<int> CurrentVote { get; private set; }
    public WeightedGenerator WeightedGenerator { get; private set; }
    #endregion


    public MapModel() {
        TileRows = new List<TileModel[]>();
        CurrentTileIndex = 0;
        CurrentVote = new List<int>();
        WeightedGenerator = new WeightedGenerator(Enum.GetNames(typeof(TileType)).Length, 1);
    }


    public TileModel[] CreateTileRow() {
        int rowSize = UnityEngine.Random.Range(2, 6);
        int[] array = WeightedGenerator.GenerateArray(rowSize);
        TileModel[] result = array.Select(x => new TileModel(TileType.Fight)).ToArray();
        //TileModel[] result = array.Select(x => new TileModel((TileType)x)).ToArray();

        return result;
    }

    public void AddTileRow(TileModel[] tileDatas) {
        TileRows.Add(tileDatas);
    }

    public void MoveToTile(VoteResult result) {
        CurrentTileIndex = result.Index;
    }

    public TileModel GetCurrentTile() {
        return TileRows.Last()[CurrentTileIndex];
    }


    //Votes
    public void CreatePlayer() {
        CurrentVote.Add(GameUtilsAndConsts.EMPTY_INT);
    }

    public void UpdateVotes(int playerId, int value, out List<int> result) {
        CurrentVote[playerId] = value;
        result = CurrentVote;
    }

    public VoteResult GetVoteResult() {
        VoteResult result = new VoteResult(false, GameUtilsAndConsts.EMPTY_INT, 0);
        TileModel[] row = TileRows.Last();

        int maxCount = CurrentVote.GroupBy(n => n).Max(g => g.Count());
        List<int> voteResults = CurrentVote.GroupBy(n => n).Where(g => g.Count() == maxCount).Select(g => g.Key).ToList();
        voteResults = voteResults.Select(n => n).Where(n => n != GameUtilsAndConsts.EMPTY_INT).ToList();

        if (voteResults.Count == 1) {
            int index = voteResults.First();
            result = new VoteResult(false, index, (int)row[index].TileType);
        }
        else if (voteResults.Count > 1) {
            voteResults.Shuffle();
            int index = voteResults.First();
            result = new VoteResult(true, index, (int)row[index].TileType);
        }
        else {
            int length = TileRows.Last().Length;
            int r = length > 1 ? UnityEngine.Random.Range(0, length) : 0;
            result = new VoteResult(length > 1, r, (int)row[r].TileType);
        }

        return result;
    }

    public void ClearVotes() {
        for (int i = 0; i < CurrentVote.Count; i++)
            CurrentVote[i] = GameUtilsAndConsts.EMPTY_INT;
    }
}
