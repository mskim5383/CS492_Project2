using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts;
using System.Collections;

namespace Assets.Scripts.Card
{
    class Deck
    {
        public Dictionary<KeyValuePair<Mark, int>, GameObject> cardMap;
        public ArrayList defaultCards;
        public Deck()
        {
            cardMap = new Dictionary<KeyValuePair<Mark, int>, GameObject>();
            defaultCards = new ArrayList();
            foreach (Mark mark in new Mark[] { Mark.S, Mark.H, Mark.D, Mark.C, Mark.JK })
            {
                if (mark != Mark.JK)
                {
                    for (int num = 1; num <= 13; num++)
                    {
                        Sprite sprite = CardSprites.getInstance().getSprite(mark, num);
                        createCardObjectWithSprite(mark, num, sprite);
                    }
                } else
                {
                    Sprite sprite = CardSprites.getInstance().getSprite(mark, 0);
                    createCardObjectWithSprite(mark, 0, sprite);
                }
            }
        }

        void createCardObjectWithSprite(Mark mark, int num, Sprite sprite)
        {
            GameObject card = GameObject.Find("CardSprite");
            KeyValuePair<Mark, int> key = new KeyValuePair<Mark, int>(mark, num);
            GameObject new_card = UnityEngine.Object.Instantiate(card);

            SpriteRenderer sr = new_card.GetComponent<SpriteRenderer>();
            sr.sprite = sprite;

            cardMap.Add(key, new_card);
        }

        private GameObject createDefaultCard()
        {
            GameObject card = GameObject.Find("CardSprite");
            GameObject new_card = UnityEngine.Object.Instantiate(card);
            SpriteRenderer sr = new_card.GetComponent<SpriteRenderer>();
            sr.sprite = CardSprites.getInstance().defaultSprite;
            return new_card;
        }

        public int last = 0;
        public GameObject getDefaultCard(int i)
        {
            while (i >= defaultCards.Count)
            {
                defaultCards.Add(createDefaultCard());
            }
            if (i > last) last = i;
            return defaultCards[i] as GameObject;
        }
        
        public List<GameObject> getRemainDefaultCards(int i)
        {
            List<GameObject> res = new List<GameObject>();
            while (i < defaultCards.Count)
            {
                res.Add(defaultCards[i] as GameObject);
            }
            return res;
        }

        public void decreaseDefaultCard(int limit)
        {
            while(last >= limit)
            {
                GameObject card = defaultCards[last] as GameObject;
                card.transform.position = new Vector3(0, 0, -20);
                last--;
            }
        }

        public GameObject getCard(Mark mark, int num)
        {
            KeyValuePair<Mark, int> key = new KeyValuePair<Mark, int>(mark, num);
            return cardMap[key];
        }
    }
}
