using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileView : MonoBehaviour {
    [SerializeField] private Image _icon;
    [SerializeField] private List<GameObject> _spots;
    [SerializeField] private Sprite[] _icons;

    private TileData _tileData;
    private ViewMap _viewMap;


    public void Init(TileData tileData, ViewMap viewMap) {
        _tileData = tileData;
        _viewMap = viewMap;
        _icon.sprite = _icons[(int)tileData.TileType];

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
            _spots[i].transform.DOScale(Vector3.one * (indexes[i] == _tileData.Index ? 1 : 0), GameUtilsAndConsts.ANIM_DURATION);
        }
    }

    private void OnClick() {
        _viewMap.ClickOnTile(_tileData.Index);
    }
}
