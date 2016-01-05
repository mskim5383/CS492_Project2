using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Card
{
    public enum Mark
    {
        Spades,
        Heart,
        Diamond,
        Club,
        Joker,
    };
    // Singleton Class
    class CardMesh
    {

        Dictionary<KeyValuePair<Mark, int>, Mesh> meshMap;
        Mesh defaultMesh;
        // initialize
        private CardMesh()
        {
            meshMap = new Dictionary<KeyValuePair<Mark, int>, Mesh>();
            string path_prefix = "Free_Playing_Cards/PlayingCards_";
            string[] num_name = new string[] {"", "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            foreach (Mark mark in new Mark[]{ Mark.Spades, Mark.Heart, Mark.Diamond, Mark.Club }) {
                for (int num = 1; num <= 13; num++)
                {
                    KeyValuePair<Mark, int> key = new KeyValuePair<Mark, int>(mark, num);
                    string path = path_prefix + num_name[num] + mark.ToString();
                    Mesh mesh = (Mesh)Resources.Load(path, typeof(Mesh));
                    meshMap.Add(key, mesh);
                }
            }
            defaultMesh = (Mesh)Resources.Load(path_prefix + "Box", typeof(Mesh));
        }

        public Mesh getMesh(Mark mark, int num)
        {
            KeyValuePair<Mark, int> key = new KeyValuePair<Mark, int>(mark, num);
            if (meshMap.ContainsKey(key))
            {
                return meshMap[key];
            }
            return defaultMesh;
        }

        public static CardMesh instance;
        public static CardMesh getInstance()
        {
            if (instance == null)
            {
                instance = new CardMesh();
            }
            return instance;
        }
        
    }
}
