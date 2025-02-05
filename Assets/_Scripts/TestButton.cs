using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestButton : MonoBehaviour {
    public int Size;
    public int WeightStep;
    public List<TileType> Tiles = new List<TileType>();
    public List<int> Results = new List<int>();
    public List<TileType> ResultsType = new List<TileType>();

    private WeightedGenerator _generator;

    public void R() {
        _generator = new WeightedGenerator(Enum.GetNames(typeof(TileType)).Count(), WeightStep);

        Tiles.Clear();
    }

    public void Test() {
        R();
        int[] array = _generator.GenerateArray(Size);

        foreach (int i in array)
            Tiles.Add((TileType)i);

        Results = Tiles.GroupBy(x => x).Select(g => g.Count()).OrderByDescending(n => n).ToList();
        ResultsType = Tiles.GroupBy(x => x).OrderByDescending(g => g.Count()).Select(n => n.Key).ToList();
    }
}
