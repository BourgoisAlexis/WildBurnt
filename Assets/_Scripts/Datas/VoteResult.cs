using System;

[Serializable]
public struct VoteResult {
    public bool Randomized;
    public int Result;

    public VoteResult(bool randomized, int result) {
        Randomized = randomized;
        Result = result;
    }
}
