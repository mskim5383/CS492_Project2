using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts;

namespace Assets.Scripts.Card
{
    class Deck
    {
        public Dictionary<KeyValuePair<Mark, int>, GameObject> cardMap;
        public Deck()
        {
            cardMap = new Dictionary<KeyValuePair<Mark, int>, GameObject>();
            foreach (Mark mark in new Mark[] { Mark.Spades, Mark.Heart, Mark.Diamond, Mark.Club, Mark.Joker })
            {
                if (mark != Mark.Joker)
                {
                    for (int num = 1; num <= 13; num++)
                    {
                        Mesh mesh = CardMesh.getInstance().getMesh(mark, num);
                        createCardObjectWithMesh(mark, num, mesh);
                    }
                } else
                {
                    Mesh mesh = CardMesh.getInstance().getMesh(mark, 0);
                    createCardObjectWithMesh(mark, 0, mesh);
                }
            }
        }
        void createCardObjectWithMesh(Mark mark, int num, Mesh mesh)
        {
            GameObject card = GameObject.Find("CardTemplate");
            KeyValuePair<Mark, int> key = new KeyValuePair<Mark, int>(mark, num);
            GameObject new_card = UnityEngine.Object.Instantiate(card);

            MeshFilter filter = new_card.GetComponent<MeshFilter>();
            filter.sharedMesh = mesh;

            cardMap.Add(key, new_card);
        }

        public GameObject getCard(Mark mark, int num)
        {
            KeyValuePair<Mark, int> key = new KeyValuePair<Mark, int>(mark, num);
            return cardMap[key];
        }
    }
}
