﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Card;
using Assets.Scripts.Table;
using LitJson;

public class MainScript : MonoBehaviour {
    // Use this for initialization

    Deck deck;
	void Start () {
        // dummy
        //GameStatus.getInstance().id = "102";
        //GameStatus.getInstance().id = "103";
        //GameStatus.getInstance().id = "104";
        //GameStatus.getInstance().id = "105";
        //GameStatus.getInstance().id = "106";
        //GameStatus.getInstance().room_id = "14";
        // --dummy
        deck = new Deck();
        WWWRequest request = new WWWRequest();
        StartCoroutine(request.RequestGameStatus());
    }

    // Update is called once per frame
    const float updateTime = 0.5f;
    float remainTime = 0.5f;
    void Update () {
        remainTime -= Time.deltaTime;

        WWWRequest request = new WWWRequest();
        if (remainTime < 0.0)
        {
            StartCoroutine(request.RequestGameStatus());
            remainTime = updateTime;
        }

        if (!GameStatus.getInstance().dirty) return;
        GameStatus.getInstance().dirty = false;
        DrawFromJson render = new DrawFromJson(deck);
        render.draw(GameStatus.getInstance().jsonData);
    }
}
