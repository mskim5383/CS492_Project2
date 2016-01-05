using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Table
{
    class TablePosition
    {
        private const int tableMargin = 30;
        private const int cardMargin = 20;
        private const int ownCardMargin = 50;
        private static int getPos(int centerPosition, int i, int n, int margin)
        {
            int startPosition = centerPosition - (margin * (n - 1)) / 2;
            return startPosition + margin * i;
        }

        class Player5_Center
        {
            int player;
            public Player5_Center(int player)
            {
                this.player = player;
            }

            public int x
            {
                get {
                    switch (player)
                    {
                        case 0:
                            return Screen.width / 2;
                        case 1:
                            return Screen.width - tableMargin;
                        case 2:
                            return Screen.width * 2 / 3;
                        case 3:
                            return Screen.width / 3;
                        case 4:
                            return tableMargin;
                    }
                    return 0;
                }
            }

            public int y
            {
                get
                {
                    switch (player)
                    {
                        case 0:
                            return tableMargin;
                        case 1:
                        case 4:
                            return Screen.height / 2;
                        case 2:
                        case 3:
                            return Screen.height - tableMargin;
                    }
                    return 0;
                }
            }

            public bool isVertical
            {
                get
                {
                    return player == 1 || player == 4;
                }
            }

            public bool isRev
            {
                get
                {
                    return player == 1 || player == 2 || player == 3;
                    //return player == 0 || player == 4;
                }
            }
        };

        // 자신부터 반시계방향으로 0, 1, 2, 3, 4
        public static Vector3 getCardPositionForHand(int player, int i, int n)
        {
            Camera camera = Camera.main;
            Player5_Center center = new Player5_Center(player);
            if (center.isRev) i = n - 1 - i;
            int x = center.x, y = center.y;
            if (center.isVertical)
            {
                y = getPos(y, i, n, player == 0 ? ownCardMargin : cardMargin);
            }
            else
            {
                x = getPos(x, i, n, player == 0 ? ownCardMargin : cardMargin);
            }
            Vector3 pos = camera.ScreenToWorldPoint(new Vector3(x, y, 10));
            pos.z = -1;
            return pos;
        }
        
        public static Vector3 getCardPositionForPointCard(int player, int i, int n)
        {
            Camera camera = Camera.main;
            Player5_Center center = new Player5_Center(player);
            if (center.isRev) i = n - 1 - i;
            int x = center.x, y = center.y;
            if (center.isVertical)
            {
                y = getPos(y, i, n, cardMargin);
                if (player == 1) x -= tableMargin;
                else x += tableMargin;
            }
            else
            {
                x = getPos(x, i, n, cardMargin);
                if (player == 0) y += tableMargin;
                else y -= tableMargin;
            }
            return camera.ScreenToWorldPoint(new Vector3(x, y, 10));
        }

        public static Vector3 getThrowPosition(int player)
        {
            Camera camera = Camera.main;
            Player5_Center center = new Player5_Center(player);
            int cx = Screen.width / 2;
            int cy = Screen.height / 2;

            int x = (center.x + cx) / 2;
            int y = (center.y + cy) / 2;

            return camera.ScreenToWorldPoint(new Vector3(x, y, 10));
        }
    }
}
