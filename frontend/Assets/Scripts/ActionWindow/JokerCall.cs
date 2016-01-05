using UnityEngine;
using System.Collections;

public class JokerCall : MonoBehaviour
{
    void OnGUI()
    {
        return;

        int cx, cy;
        cx = Screen.width / 2;
        cy = Screen.height / 2;

        GUI.Box(new Rect(cx - 50, cy - 30, 100, 60), "조커콜?");
        if (GUI.Button(new Rect(cx - 30, cy, 20, 20), "Y"))
        {

        }
        if (GUI.Button(new Rect(cx + 10, cy, 20, 20), "N"))
        {

        }
    }
}