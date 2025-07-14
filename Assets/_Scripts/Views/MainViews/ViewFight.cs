using System.Collections.Generic;
using UnityEngine;

public class ViewFight : ViewWildBurnt {
	#region Variables
	[SerializeField] private RectTransform _top;
	[SerializeField] private RectTransform _bot;

	[SerializeField] private GameObject _prefab;

	private List<MobView> _views;
	#endregion


	private void Awake() {
		_views = new List<MobView>();
	}


	private void Clear() {
		foreach (MobView view in _views)
			Destroy(view.gameObject);

		_views.Clear();
	}

	public void CreateBoard(CharacterModel[][] fullBoard) {
		CharacterModel[] players = fullBoard[0];
		CharacterModel[] mobs = fullBoard[1];

		Clear();

		AddCharacters(mobs, true, 0);
		AddCharacters(players, false, mobs.Length);
	}

	public void AddCharacters(CharacterModel[] characters, bool top, int indexOffset) {
		int size = characters.Length;

		Vector2 midSize = _rectTransform.rect.size;
		float ratio = _top.anchorMin.y - _bot.anchorMax.y;
		midSize.y *= ratio;

		//Horizontal
		float stepH = midSize.x / (float)(size + 1);
		float offsetH = midSize.x / 2 - stepH;

		//Vertical
		float stepV = midSize.y / 4;

		for (int i = 0; i < size; i++) {
			Vector2 pos = new Vector2(i * stepH - offsetH, stepV * (top ? 1 : -1));
			_views.Add(AddSingleItem(pos, characters[i], i + indexOffset));
		}
	}

	//todo : refacto with common elements in ViewLoot.cs
	private MobView AddSingleItem(Vector2 position, CharacterModel model, int index) {
		GameObject go = Instantiate(_prefab, _rectTransform);
		MobView view = go.GetComponent<MobView>();

		go.RectScaleIn();
		go.transform.localPosition = position;

		view.Init(model, index);
		view.OnClick.AddListener(ClickOnMob);
		view.SetInteractable(true);

		return view;
	}


	//Actions
	public void ClickOnMob(int index) {
		_gameView.ClickOnMob(index);
	}
}
