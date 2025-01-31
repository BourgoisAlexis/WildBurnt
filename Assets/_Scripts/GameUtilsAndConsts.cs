using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtilsAndConsts {
    public static float ANIM_DURATION = 0.15f;  //See uielement consts

    public static int SHOWING_ROWS = 2;
    public static int EMPTY_VOTE = -1;
    public static string EMPTY_MESSAGE = "Empty";
    public static float MESSAGE_DELAY = 0.3f;

    public static List<MessageType> DONT_SEND_TO_SELF = new List<MessageType> {
        MessageType.Default,

        MessageType.AskForID,
        MessageType.GiveID,
    };


    public static Color ColorFromPlayerID(int index) {
        if (index == 0)
            return Color.red;
        else if (index == 1)
            return Color.blue;
        else if (index == 2)
            return Color.green;
        else if (index == 3)
            return Color.yellow;

        return Color.magenta;
    }

    public static TileData[] CreateTileRow() {
        int randomSize = System.Enum.GetNames(typeof(TileType)).Length;
        int rowSize = 4;//Random.Range(1, 5);
        TileData[] result = new TileData[rowSize];

        for (int i = 0; i < rowSize; i++)
            result[i] = new TileData((TileType)Random.Range(1, randomSize), i);

        return result;
    }


    //Extension methods
    public static void AnimateRectTransform(this GameObject go) {
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.localPosition = Vector3.zero;
        rect.localScale = Vector3.zero;
        rect.DOScale(Vector3.one, ANIM_DURATION);
    }
}
