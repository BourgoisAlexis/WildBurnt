using System.Collections.Generic;

public struct PlayerModel {
    public int ID;
    public string UserName;

    public PlayerModel(int id) {
        ID = id;
        UserName = $"Player_{id}";
    }
}


public class GameModel {
    private List<PlayerModel> _playerModels = new List<PlayerModel>();
    private List<int> _currentVote = new List<int>();


    public int CreatePlayer() {
        _playerModels.Add(new PlayerModel(_playerModels.Count));
        _currentVote.Add(GameConsts.EMPTY_VOTE);
        return _playerModels.Count - 1;
    }

    public List<int> Vote(int playerID, int value) {
        _currentVote[playerID] = value;
        return _currentVote;
    }

    public void ClearVotes() {
        for (int i = 0; i < _currentVote.Count; i++)
            _currentVote[i] = GameConsts.EMPTY_VOTE;
    }
}
