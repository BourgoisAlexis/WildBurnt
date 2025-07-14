using UnityEngine.Events;
using UnityEngine.EventSystems;


public class MobView : UIButtonAbstract<UnityEvent<int>> {
	#region Variables
	private CharacterModel _model;
	private int _index;
	#endregion


	public void Init(CharacterModel characterModel, int index) {
		_model = characterModel;
		_index = index;
		_text.text = characterModel.Name;

		SetInteractable(true);
	}

	public override void OnPointerClick(PointerEventData eventData) {
		OnClick?.Invoke(_index);
	}
}
