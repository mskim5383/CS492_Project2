using UnityEngine;
using System.Collections;
using Assets.Scripts.Card;
using Assets.Scripts;
using System;
using LitJson;
using Assets.Scripts.Table;
using Assets.Scripts.ActionWindow;

public class Contract : MonoBehaviour {
    Mark selected;
    int choosed = 0;
    void OnGUI()
    {
        GameStatus status = GameStatus.getInstance();
        if (status.nowStatus != 1) return;
        bool myTurn = CommonFunction.isMyTurn();
        int contractOrder = -1;
        string contractMark = "";
        int contractNum = -1;
        try
        {
            int myorder = (int)status.jsonData["player"]["order"];
            JsonData contract = status.jsonData["game_status"]["contract"];
            if (contract != null)
            {
                contractMark = contract["face"].ToString();
                contractNum = (int)contract["number"];
                contractOrder = ((int)contract["order"] - myorder + 5) % 5;
            }
        }
        catch (Exception) { }
        if (contractOrder>=0)
        {
            TablePosition.Player5_Center center = new TablePosition.Player5_Center(contractOrder);
            int cx = center.x;
            int cy = Screen.height - center.y;

            cx = (cx + Screen.width/2) / 2;
            cy = (cy + Screen.height/2) / 2;

            GUI.Box(new Rect(cx - 25, cy - 25, 50, 50), contractMark + contractNum.ToString());
        }
        if (myTurn)
        {
            int cx, cy;
            cx = Screen.width / 2;
            cy = Screen.height / 2;
            GUI.Box(new Rect(cx - 175, cy - 80, 350, 160), "공약 설정");
            int mx = cx - 75;
            foreach (Mark mark in new Mark[] { Mark.S, Mark.D, Mark.H, Mark.C })
            {
                Color bef = GUI.color;
                if (selected == mark)
                {
                    GUI.color = new Color(255, 0, 0);
                }
                if (GUI.Button(new Rect(mx, cy - 40, 30, 30), mark.ToString()))
                {
                    selected = mark;
                }
                GUI.color = bef;
                mx += 40;
            }

            int s = (int)status.jsonData["game_status"]["contract_limit"], e = 20;
            int n = e - s + 1;
            int wall = 40 * (n - 1) + 30;
            mx = cx - wall / 2;
            for (int i = s; i <= e; i++)
            {
                Color bef = GUI.color;
                if (i == choosed)
                {
                    GUI.color = new Color(255, 0, 0);
                }
                if (GUI.Button(new Rect(mx, cy, 30, 30), i.ToString()))
                {
                        choosed = i;
                }
                GUI.color = bef;
                mx += 40;
            }

            WWWRequest request = new WWWRequest();
            if (GUI.Button(new Rect(cx - 100, cy + 40, 50, 30), "OK"))
            {
                StartCoroutine(request.RequestContract(false, selected.ToString(), choosed));
            }
            if (GUI.Button(new Rect(cx + 50, cy + 40, 50, 30), "Pass"))
            {
                StartCoroutine(request.RequestContract(true));
            }
        }
    }
}
