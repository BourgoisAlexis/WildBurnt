using DG.Tweening;
using UnityEngine;

public static class GameUtils {
    public static TileData[] CreateTileRow() {
        int size = Random.Range(1, 5);
        TileData[] result = new TileData[size];

        for (int i = 0; i < size; i++)
            result[i] = new TileData(Random.Range(0, 5));

        return result;
    }

    public static void AnimateRectTransform(this GameObject go) {
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.localPosition = Vector3.zero;
        rect.localScale = Vector3.zero;
        rect.DOScale(Vector3.one, GameConsts.ANIM_DURATION);
    }
}
