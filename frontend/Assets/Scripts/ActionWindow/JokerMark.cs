using UnityEngine;
using System.Collections;
using Assets.Scripts.Card;

public class JokerMark : MonoBehaviour {

    void OnGUI()
    {
        return;

        int cx, cy;
        cx = Screen.width / 2;
        cy = Screen.height / 2;
        GUI.Box(new Rect(cx - 100, cy - 60, 200, 120), "조커 문양 설정");
        int mx = cx - 75;
        foreach (Mark mark in new Mark[] { Mark.S, Mark.D, Mark.H, Mark.C })
        {
            if (GUI.Button(new Rect(mx, cy - 30, 30, 30), mark.ToString()))
            {
            }
            mx += 40;
        }
        if (GUI.Button(new Rect(cx - 50, cy + 10, 100, 30), "설정안함"))
        {
        }
    }
}
