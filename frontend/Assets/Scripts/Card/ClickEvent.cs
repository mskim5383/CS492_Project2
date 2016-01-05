using UnityEngine;
using System.Collections;
using Assets.Scripts.Card;
using Assets.Scripts;

public class ClickEvent : MonoBehaviour {
    public bool selected = false;
    void OnMouseDown()
    {
        selected = !selected;

        GameStatus.getInstance().dirty = true;
    }
}