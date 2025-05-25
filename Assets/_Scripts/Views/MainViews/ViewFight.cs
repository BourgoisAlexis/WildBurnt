using System.Collections.Generic;
using UnityEngine;

public class ViewFight : ViewWildBurnt {
    #region Variables
    [SerializeField] private RectTransform _top;
    [SerializeField] private RectTransform _bot;

    [SerializeField] private GameObject _prefab;

    private List<ItemView> _itemViews;
    #endregion


    private void Awake() {
        _itemViews = new List<ItemView>();
    }
}
