using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileView : MonoBehaviour {
    [SerializeField] private List<GameObject> _spots;

    private TileData _tileData;
    private int _index;
    private GameView _gameView;


    public void Init(TileData tileData, int index, GameView gameView) {
        _tileData = tileData;
        _index = index;
        _gameView = gameView;

        foreach (GameObject spot in _spots)
            spot.transform.localScale = Vector3.zero;

        GetComponent<UIButtonSimple>().OnClick.AddListener(OnClick);
    }


    public void SetInteractable(bool interactable) {
        GetComponent<UIButtonSimple>().SetInteractable(interactable);
    }

    public void DisplayVotes(List<int> indexes) {
        for (int i = 0; i < indexes.Count; i++) {
            _spots[i].GetComponent<Image>().color = GameUtilsAndConsts.ColorFromPlayerID(i);
            _spots[i].transform.DOScale(Vector3.one * (indexes[i] == _index ? 1 : 0), GameUtilsAndConsts.ANIM_DURATION);
        }
    }

    private void OnClick() {
        _gameView.ClickOnTile(_index);
    }
}
