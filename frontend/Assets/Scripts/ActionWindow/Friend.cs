using UnityEngine;
using System.Collections;
using Assets.Scripts.Card;

public class Friend : MonoBehaviour
{
    public Vector2 scrollPosition = Vector2.zero;
    void OnGUI()
    {
        return;

        int cx, cy;
        cx = Screen.width / 2;
        cy = Screen.height / 2;
        GUI.Box(new Rect(cx - 175, cy - 100, 350, 200), "프렌드와 버릴카드 세장을 선택해주세요.");
        scrollPosition = GUI.BeginScrollView(new Rect(cx - 150, cy - 80, 300, 80), scrollPosition, new Rect(0, 0, 530, 170));
        int my = 10;
        foreach (Mark mark in new Mark[] { Mark.S, Mark.D, Mark.H, Mark.C })
        {
            int mx = 10;
            string[] num_name = new string[] { "", "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            for (int c=1;c<=13;c++)
            {
                string strCard = mark.ToString() + num_name[c];
                if (GUI.Button(new Rect(mx, my, 30, 30), strCard))
                {

                }
                mx += 40;
            }
            my += 40;
        }
        GUI.EndScrollView();

        int ex = cx - 150;
        if (GUI.Button(new Rect(ex, cy + 10, 50, 30), "조커"))
        {
        }
        ex += 50;
        if (GUI.Button(new Rect(ex, cy + 10, 50, 30), "초구"))
        {
        }
        ex += 50;
        if (GUI.Button(new Rect(ex, cy + 10, 50, 30), "지정1"))
        {
        }
        ex += 50;

        if (GUI.Button(new Rect(ex, cy + 10, 50, 30), "지정2"))
        {
        }
        ex += 50;
        if (GUI.Button(new Rect(ex, cy + 10, 50, 30), "지정3"))
        {

        }
        ex += 50;
        if (GUI.Button(new Rect(ex, cy + 10, 50, 30), "지정4"))
        {

        }
        ex += 50;

        if (GUI.Button(new Rect(cx - 50, cy + 50, 100, 30), "결정"))
        {

        }
    }
}
