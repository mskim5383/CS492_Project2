using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Table
{
    class TablePosition
    {
        // 자신부터 반시계방향으로 0, 1, 2, 3, 4
        public static Vector3 getCardPositionForHand(int player, int i, int n)
        {
            Camera camera = Camera.main;
            if (player == 0)
            {
                int sx = Screen.width / 2;
                int d = 20;
                sx -= (d*(n-1))/ 2;
                int x = sx + d * i;
                int y = 30;

                Vector3 pos = camera.ScreenToWorldPoint(new Vector3(x, y, 10));
                return pos;
            }

            return new Vector3(0, 0, 0);
        }
    }
}
