using Assets.Scripts.ActionWindow;
using Assets.Scripts.Card;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Table
{
    class DrawFromJson
    {
        Deck deck;
        public DrawFromJson(Deck deck) {
            this.deck = deck;
        }

        public bool changeCardColor(GameObject cardObject, bool forcedWhite = false)
        {
            ClickEvent c = cardObject.GetComponent<ClickEvent>();
            SpriteRenderer sr = cardObject.GetComponent<SpriteRenderer>();
            if (forcedWhite) c.selected = false;
            if (c.selected)
            {
                sr.color = new Color(0, 255, 0);
                return true;
            }
            else
            {
                sr.color = new Color(255, 255, 255);
                return false;
            }
        }

        public void draw(JsonData data)
        {
            Dictionary<GameObject, bool> used = new Dictionary<GameObject, bool>();
            foreach (GameObject card in deck.cardMap.Values)
            {
                used.Add(card, false);
            }

            int back_count = 0;
            JsonData game_status = data["game_status"];
            int status = (int)game_status["status"];
            if (status == 0) return;
            JsonData player = data["player"];

            JsonData hands = player["hands"];
            int OWN = 0;
            int count = 0;
            string selectedStr = "";
            for (int i=0;i<hands.Count ;i++)
            {
                string strCard = hands[i].ToString();
                GameObject cardObject = getCardObjectByString(strCard);
                used[cardObject] = true;
                if (changeCardColor(cardObject))
                {
                    if (count > 0) selectedStr += ",";
                    selectedStr += strCard;
                    count++;
                }
                cardObject.transform.position = TablePosition.getCardPositionForHand(OWN, i, hands.Count);
                SpriteRenderer sr = cardObject.GetComponent<SpriteRenderer>();
                sr.sortingOrder = 100+i;
            }
            if (GameStatus.getInstance().remain)
            {
                if (count == 3)
                {
                    WWWRequest request = new WWWRequest();
                    GameStatus.getInstance().remainFrom.StartCoroutine(request.RequestRemain(selectedStr));
                }
                GameStatus.getInstance().remain = false;
                GameStatus.getInstance().remainFrom = null;
            }
            if (GameStatus.getInstance().card)
            {
                if (count == 1)
                {
                    WWWRequest request = new WWWRequest();
                    bool isLead = CommonFunction.isLead();
                    if (isLead && CommonFunction.isJC(selectedStr))
                    {
                        GameStatus.getInstance().jcStr = selectedStr;
                        GameStatus.getInstance().jcMark = true;
                    }
                    else if (isLead && selectedStr == "Jk")
                    {
                        GameStatus.getInstance().jkMark = true;
                    }
                    else
                    {
                        GameStatus.getInstance().cardFrom.StartCoroutine(request.RequestThrow(selectedStr));
                    }
                }
                GameStatus.getInstance().card = false;
                GameStatus.getInstance().cardFrom = null;
            }

            JsonData players = game_status["players"];
            int player_order = (int)player["order"];
            int order = player_order;

            for (int i=0;i<5;i++)
            {
                string player_name = "player" + order.ToString();
                JsonData player_data = players[player_name];
                if (i != 0)
                {
                    int hand_count = (int)player_data["hands"];
                    for (int c=0;c< hand_count;c ++)
                    {
                        GameObject cardObject = deck.getDefaultCard(back_count++);
                        changeCardColor(cardObject, true);
                        cardObject.transform.position = TablePosition.getCardPositionForHand(i, c, hand_count);
                        SpriteRenderer sr = cardObject.GetComponent<SpriteRenderer>();
                        sr.sortingOrder = c;
                    }
                }
                JsonData point_card = player_data["point_card"];
                for (int c=0;c<point_card.Count;c++)
                {
                    GameObject cardObject = getCardObjectByString(point_card[c].ToString());
                    used[cardObject] = true;
                    changeCardColor(cardObject, true);
                    cardObject.transform.position = TablePosition.getCardPositionForPointCard(i, c, point_card.Count);
                    SpriteRenderer sr = cardObject.GetComponent<SpriteRenderer>();
                    sr.sortingOrder = c;
                }
                order = (order + 1) % 5;
            }

            JsonData trick = game_status["trick"];
            for (int i=0;i< trick.Count;i++)
            {
                int trick_order = (int)trick[i]["order"];
                int rela_order = (trick_order - player_order + 5) % 5;
                string strCard = trick[i]["card"].ToString();

                GameObject cardObject = getCardObjectByString(strCard);
                used[cardObject] = true;
                changeCardColor(cardObject, true);
                cardObject.transform.position = TablePosition.getThrowPosition(rela_order);
                SpriteRenderer sr = cardObject.GetComponent<SpriteRenderer>();
                sr.sortingOrder = i;
            }


            foreach (GameObject card in deck.cardMap.Values)
            {
                if (!used[card])
                {
                    card.transform.position = new Vector3(0, 0, -20);
                }
            }

            deck.decreaseDefaultCard(back_count);
        }

        public GameObject getCardObjectByString(string strCard)
        {
            Mark mark = Mark.JK;
            int num = 0;
            
            switch(strCard[0])
            {
                case 'S':
                    mark = Mark.S;
                    break;
                case 'D':
                    mark = Mark.D;
                    break;
                case 'H':
                    mark = Mark.H;
                    break;
                case 'C':
                    mark = Mark.C;
                    break;
                case 'J':
                    mark = Mark.JK;
                    break;
            }
            if (mark != Mark.JK)
            {
                if ('1' <= strCard[1] && strCard[1] <= '9')
                {
                    num = (int)(strCard[1] - '0');
                    if (strCard.Length > 2)
                    {
                        num = num * 10 + (int)(strCard[2] - '0');
                    }
                } else
                {
                    switch(strCard[1])
                    {
                        case 'A':
                            num = 1;
                            break;
                        case 'J':
                            num = 11;
                            break;
                        case 'Q':
                            num = 12;
                            break;
                        case 'K':
                            num = 13;
                            break;
                    }
                }
            }

            KeyValuePair<Mark, int> key = new KeyValuePair<Mark, int>(mark, num);
            return deck.cardMap[key];
        }
    }
}
