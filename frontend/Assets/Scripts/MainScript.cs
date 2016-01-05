using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Card;
using Assets.Scripts.Table;

public class MainScript : MonoBehaviour {
    // Use this for initialization
    GameStatus status;
    WWWRequest request;

    Deck deck;
	void Start () {
        status = new GameStatus();
        request = new WWWRequest();
        deck = new Deck();

        StartCoroutine(request.RequestUser());

        KeyValuePair<Mark, int>[] cards = new KeyValuePair<Mark, int>[deck.cardMap.Keys.Count];
        deck.cardMap.Keys.CopyTo(cards, 0);
        int n = 10;
        for (int i=0;i<n;i++)
        {
            GameObject cardObject = deck.cardMap[cards[i]];
            cardObject.transform.position = TablePosition.getCardPositionForHand(0, i, n);
            if (Random.Range(0,2) == 0)
            {
                cardObject.transform.rotation = new Quaternion(180, 0, 0, 0);
            }
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
