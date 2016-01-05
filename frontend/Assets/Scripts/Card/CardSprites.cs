using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Card
{
    public enum Mark
    {
        S,
        H,
        D,
        C,
        JK,
        /*
        Spades,
        Heart,
        Diamond,
        Club,
        Joker,
        */
    };
    // Singleton Class
    class CardSprites
    {

        Dictionary<KeyValuePair<Mark, int>, Sprite> spriteMap;
        public Sprite defaultSprite;
        // initialize
        private CardSprites()
        {
            spriteMap = new Dictionary<KeyValuePair<Mark, int>, Sprite>();
            string path_prefix = "CardImages/";
            string[] num_name = new string[] {"", "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            foreach (Mark mark in new Mark[]{ Mark.S, Mark.H, Mark.D, Mark.C }) {
                for (int num = 1; num <= 13; num++)
                {
                    KeyValuePair<Mark, int> key = new KeyValuePair<Mark, int>(mark, num);
                    string path = path_prefix + mark.ToString() + num_name[num];
                    Sprite sprite = (Sprite)Resources.Load(path, typeof(Sprite));
                    spriteMap.Add(key, sprite);
                }
            }
            { // Joker
                KeyValuePair<Mark, int> key = new KeyValuePair<Mark, int>(Mark.JK, 0);
                string path = path_prefix + "JK";
                Sprite sprite = (Sprite)Resources.Load(path, typeof(Sprite));
                spriteMap.Add(key, sprite);
            }

            defaultSprite = (Sprite)Resources.Load(path_prefix + "BACK", typeof(Sprite));
        }

        public Sprite getSprite(Mark mark, int num)
        {
            KeyValuePair<Mark, int> key = new KeyValuePair<Mark, int>(mark, num);
            if (spriteMap.ContainsKey(key))
            {
                return spriteMap[key];
            }
            return defaultSprite;
        }

        public static CardSprites instance;
        public static CardSprites getInstance()
        {
            if (instance == null)
            {
                instance = new CardSprites();
            }
            return instance;
        }
        
    }
}
