using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TransitionView : MonoBehaviour {
    [SerializeField] private Image _mainVisual;

    public async Task Transition(bool transiIn, VoteResult result) {
        _mainVisual.sprite = DataLoader.Instance.LoadTileSprite(result.Value);
        Transform t = _mainVisual.transform;

        t.localScale = Vector3.one * (transiIn ? 0 : 100);
        await t.DOScale(transiIn ? 100 : 0, 0.5f).SetEase(transiIn ? Ease.InExpo : Ease.OutExpo).AsyncWaitForCompletion();
    }
}
