using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    class GameStatus
    {
        public static GameStatus instance;

        public string id;

        public bool dirty = true;

        public ArrayList rooms;
        public ArrayList roomMemCounts;
        public string room_id;

        public GameStatus ()
        {
            id = null;
            rooms = new ArrayList();
            roomMemCounts = new ArrayList();
            room_id = null;
        }
        public static GameStatus getInstance()
        {
            if (instance == null)
            {
                instance = new GameStatus();
            }
            return instance;
        }
    }
}
