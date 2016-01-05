using UnityEngine;
using System.Collections;
using Assets.Scripts;
using LitJson;
using System;

public class Message : MonoBehaviour
{
    void OnGUI()
    {
        JsonData jsonData = GameStatus.getInstance().jsonData;
        try {
            if ((int)jsonData["game_status"]["status"] == 0)
            {
                throw new Exception("");
            }
        } catch (Exception)
        {
            int cx = Screen.width / 2;
            int cy = Screen.height / 2;
            GUI.Box(new Rect(cx - 50, cy - 20, 100, 40), "준비중입니다.");
        }
    }
}
