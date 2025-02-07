using System;

[Serializable]
public struct VoteResult {
    public bool Randomized;
    public int Index;
    public int Value;


    public VoteResult(bool randomized, int index, int value) {
        Randomized = randomized;
        Index = index;
        Value = value;
    }
}
