using System.Collections.Generic;
using UnityEngine;

public static class GameUtilsAndConsts {
    public static float ANIM_DURATION = 0.15f;  //See uielement consts

    public static int SHOWING_ROWS = 2;
    public static int EMPTY_INT = -1;
    public static string EMPTY_MESSAGE = "Empty";
    public static float MESSAGE_DELAY = 0.3f;

    public static List<MessageType> SENT_TO_GUEST_ONLY = new List<MessageType> {
        MessageType.Default,

        MessageType.GiveId,
    };

    //These are the messages used to update the gameModel on host side
    //As long as we don't display anything on update we don't need to send them to guests
    //But they can be sent by the host to itself
    public static List<MessageType> SENT_TO_HOST_ONLY = new List<MessageType> {
        MessageType.AskForId,

        MessageType.VoteEnd,

        MessageType.Ready,

        //Actions
        MessageType.TakeLoot,
        MessageType.EquipGear,
        MessageType.UseAbility,
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

    //Extension methods
    public static void AnimateRectTransform(this GameObject go) {
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.localPosition = Vector3.zero;
        rect.localScale = Vector3.zero;
        HomeTween.TweenLocalScale(rect, Vector3.one, new SimpleTweenSettings()).FireAndForget();
    }


    public static string ColorString(this string s) {
        return $"<color=cyan>{s}</color>";
    }
}
