using System.Collections.Generic;
using UnityEngine;


public class ViewLoot : ViewWildBurnt {
	#region Variables
	[SerializeField] private RectTransform _top;
	[SerializeField] private RectTransform _bot;

	[SerializeField] private GameObject _prefab;

	private List<ItemView> _views;
	#endregion


	private void Awake() {
		_views = new List<ItemView>();
	}


	private void Clear() {
		foreach (ItemView view in _views)
			Destroy(view.gameObject);

		_views.Clear();
	}

	public void AddLoots(int[] itemIds) {
		Clear();

		int size = itemIds.Length;

		Vector2 midSize = _rectTransform.rect.size;
		float ratio = _top.anchorMin.y - _bot.anchorMax.y;
		midSize.y *= ratio;

		//Horizontal
		float stepH = midSize.x / (float)(size + 1);
		float offsetH = midSize.x / 2 - stepH;

		for (int i = 0; i < size; i++) {
			Vector2 pos = new Vector2(i * stepH - offsetH, 0);
			_views.Add(AddSingleItem(pos, DataLoader.Instance.LoadItemModel(itemIds[i]), i));
		}
	}

	private ItemView AddSingleItem(Vector2 position, ItemModel model, int index) {
		GameObject go = Instantiate(_prefab, _rectTransform);
		ItemView view = go.GetComponent<ItemView>();

		go.RectScaleIn();
		go.transform.localPosition = position;

		view.Init(model, index);
		view.OnClick.AddListener(ClickOnLoot);
		view.SetInteractable(true);

		return view;
	}

	public void LootTaken(int index) {
		_views[index].SetInteractable(false);
	}


	//Actions
	public void ClickOnLoot(int index) {
		_gameView.ClickOnLoot(index);
	}
}
