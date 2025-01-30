using DG.Tweening;
using UnityEngine;

public class GameConsts {
    //UI
    public static float ANIM_DURATION = 0.1f;
    public static Ease ANIM_EASE = Ease.InOutSine;
    public static Color GetColor(int index) {
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

    //GAME
    public static int SHOWING_ROWS = 2;
    public static int EMPTY_VOTE = -1;
}
