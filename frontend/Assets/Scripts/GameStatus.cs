using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using System.Collections;

namespace Assets.Scripts
{
    class GameStatus
    {
        public string id;
        public static GameStatus instance;

        public ArrayList rooms;

        public GameStatus ()
        {
            id = null;
            rooms = new ArrayList();
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
