using UnityEngine;
using System.Collections;
using Assets.Scripts.Card;
using Assets.Scripts.ActionWindow;
using Assets.Scripts;

public class JokerMark : MonoBehaviour {

    void OnGUI()
    {
        GameStatus status = GameStatus.getInstance();
        if (status.nowStatus != 3) return;
        if (!CommonFunction.isMyTurn()) return;
        if (!status.jkMark) return;

        int cx, cy;
        cx = Screen.width / 2;
        cy = Screen.height / 2;
        GUI.Box(new Rect(cx - 100, cy - 60, 200, 120), "조커 문양 설정");
        int mx = cx - 75;
        WWWRequest request = new WWWRequest();
        foreach (Mark mark in new Mark[] { Mark.S, Mark.D, Mark.H, Mark.C })
        {
            if (GUI.Button(new Rect(mx, cy - 30, 30, 30), mark.ToString()))
            {
                StartCoroutine(request.RequestThrow("Jk", mark.ToString()));
                status.jkMark = false;
            }
            mx += 40;
        }
        if (GUI.Button(new Rect(cx - 50, cy + 10, 100, 30), "설정안함"))
        {
            StartCoroutine(request.RequestThrow("Jk", "_"));
            status.jkMark = false;
        }
    }
}
