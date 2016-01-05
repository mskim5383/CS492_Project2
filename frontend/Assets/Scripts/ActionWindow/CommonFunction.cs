using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.ActionWindow
{
    class CommonFunction
    {
        public static bool isMyTurn()
        {
            GameStatus status = GameStatus.getInstance();
            bool myTurn = false;

            try
            {
                int myorder = (int)status.jsonData["player"]["order"];
                myTurn = (int)status.jsonData["game_status"]["turn"] == myorder;
            }
            catch (Exception) { }

            return myTurn;
        }

        public static bool isJC(string S)
        {
            GameStatus status = GameStatus.getInstance();
            bool isJc = false;
            try
            {
                string giru = status.jsonData["game_status"]["contract"]["face"].ToString();
                if (giru == "C")
                {
                    isJc = S == "S3";
                } else
                {
                    isJc = S == "C3";
                }
            }
            catch (Exception) { }
            return isJc;
        }

        public static bool isLead()
        {

            GameStatus status = GameStatus.getInstance();
            bool isLead = false;
            try
            {
                int leadOrder = (int)status.jsonData["game_status"]["lead"];
                int myorder = (int)status.jsonData["player"]["order"];
                isLead = leadOrder == myorder;
            }
            catch (Exception) { }

            return isLead;
        }
    }
}
