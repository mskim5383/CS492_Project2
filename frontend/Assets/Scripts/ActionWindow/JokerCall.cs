using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.ActionWindow;

public class JokerCall : MonoBehaviour
{
    void OnGUI()
    {
        GameStatus status = GameStatus.getInstance();
        if (status.nowStatus != 3) return;
        if (!CommonFunction.isMyTurn()) return;
        if (!status.jcMark) return;

        int cx, cy;
        cx = Screen.width / 2;
        cy = Screen.height / 2;

        GUI.Box(new Rect(cx - 50, cy - 30, 100, 60), "조커콜?");
        WWWRequest request = new WWWRequest();
        if (GUI.Button(new Rect(cx - 30, cy, 20, 20), "Y"))
        {
            StartCoroutine(request.RequestJC(status.jcStr));
            status.jcMark = false;
        }
        if (GUI.Button(new Rect(cx + 10, cy, 20, 20), "N"))
        {
            StartCoroutine(request.RequestThrow(status.jcStr));
            status.jcMark = false;
        }
    }
}