using UnityEngine;
using System.Collections;
using Assets.Scripts.ActionWindow;
using Assets.Scripts;

public class Myturn : MonoBehaviour
{

    void OnGUI()
    {
        GameStatus status = GameStatus.getInstance();
        if (status.nowStatus != 3) return;
        if (!CommonFunction.isMyTurn()) return;
        if (status.jcMark || status.jkMark) return;

        if (status.card) return;

        int cx = Screen.width / 2;
        int cy = Screen.height / 2;
        if (GUI.Button(new Rect(cx-50, cy-25, 100, 50), "카드내기"))
        {
            status.card = true;
            status.cardFrom = this;
            status.dirty = true;
        }
    }
}
