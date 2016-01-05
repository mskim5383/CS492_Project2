using UnityEngine;
using System.Collections;
using Assets.Scripts.Card;

public class Contract : MonoBehaviour {
    void OnGUI()
    {
        return;

        int cx, cy;
        cx = Screen.width / 2;
        cy = Screen.height / 2;
        GUI.Box(new Rect(cx-175, cy-80, 350, 160), "공약 설정");
        int mx = cx - 75;
        foreach (Mark mark in new Mark[] {Mark.S, Mark.D, Mark.H, Mark.C })
        {
            if (GUI.Button(new Rect(mx, cy - 40, 30, 30), mark.ToString()))
            {
            }
            mx += 40;
        }

        int s = 13, e = 20;
        int n = e - s + 1;
        int wall = 40 * (n - 1) + 30;
        mx = cx - wall / 2;
        for (int i = s; i <= e;i++)
        {
            if (GUI.Button(new Rect(mx, cy, 30, 30), i.ToString()))
            {
            }
            mx += 40;
        }

        if (GUI.Button(new Rect(cx - 100, cy + 40, 50, 30), "OK"))
        {

        }
        if (GUI.Button(new Rect(cx + 50, cy + 40, 50, 30), "Pass"))
        {

        }
    }
}
