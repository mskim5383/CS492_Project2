using UnityEngine;
using System.Collections;
using Assets.Scripts.Card;
using Assets.Scripts;
using Assets.Scripts.ActionWindow;

public class Friend : MonoBehaviour
{
    public Vector2 scrollPosition = Vector2.zero;
    Mark sMark;
    int sC;
    int friend;
    int select;
    void OnGUI()
    {
        GameStatus status = GameStatus.getInstance();
        Color bef;
        if (status.nowStatus != 2) return;
        if (!CommonFunction.isMyTurn()) return;

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
                bef = GUI.color;
                if (friend == 2 && sMark == mark && c == sC)
                {
                    GUI.color = new Color(255, 0, 0);
                }
                string strCard = mark.ToString() + num_name[c];
                if (GUI.Button(new Rect(mx, my, 30, 30), strCard))
                {
                    sMark = mark;
                    sC = c;
                    friend = 2;
                }
                mx += 40;
                GUI.color = bef;
            }
            my += 40;
        }
        GUI.EndScrollView();

        int ex = cx - 150;

        bef = GUI.color;
        if (friend == 2 && sMark == Mark.JK)
        {
            GUI.color = new Color(255, 0, 0);
        }
        if (GUI.Button(new Rect(ex, cy + 10, 50, 30), "조커"))
        {
            sMark = Mark.JK;
            friend = 2;
        }
        GUI.color = bef;

        ex += 50;
        
        bef = GUI.color;
        if (friend == 1)
        {
            GUI.color = new Color(255, 0, 0);
        }
        if (GUI.Button(new Rect(ex, cy + 10, 50, 30), "초구"))
        {
            friend = 1;
        }
        GUI.color = bef;
        int myorder = (int)status.jsonData["player"]["order"];
        for (int a = 1; a <= 4; a++)
        {
            int aorder = (myorder + a) % 5;
            bef = GUI.color;
            if (friend == 3 && (aorder == select))
            {
                GUI.color = new Color(255, 0, 0);
            }
            ex += 50;
            if (GUI.Button(new Rect(ex, cy + 10, 50, 30), "지정" + a.ToString()))
            {
                friend = 3;
                select = aorder;
            }
            GUI.color = bef;
        }

        if (GUI.Button(new Rect(cx - 50, cy + 50, 100, 30), "결정"))
        {
            WWWRequest request = new WWWRequest();
            if (1 <= friend && friend <= 3)
            {
                StartCoroutine(request.RequestFriend(friend, sMark, sC, select));
            }
            GameStatus.getInstance().remain = true;
            GameStatus.getInstance().remainFrom = this;
            GameStatus.getInstance().dirty = true;
        }
    }
}
