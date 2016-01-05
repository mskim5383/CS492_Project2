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
        
        public bool dirty = false;
        public bool remain = false;
        public Friend remainFrom;
        public bool card = false;
        public Myturn cardFrom;
        public bool jkMark = false;
        public string jcStr = "";
        public bool jcMark = false;

        public string id;

        public ArrayList rooms;
        public ArrayList roomMemCounts;
        public string room_id;

        public JsonData jsonData;
        public int nowStatus = -1;

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
