﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandServer
{
    public static class Game
    {
        public static int Turn;
        public static List<KeyValuePair<Guid, Action>> ActionQueue = new List<KeyValuePair<Guid, Action>>();

        public static List<Player> Players = new List<Player>();
        public static bool Started = false;

        public static Map Map = new Map(300, 300, 0);

        public static List<Point> GetSpawnLocations()
        {
            List<Point> spawnPoints = new List<Point>();
            for (int y = 0; y < Map.Height; y++)
            {
                for (int x = 0; x < Map.Width; x++)
                {
                    if (Map.Tiles[y][x] == (int)Tile.Spawn)
                    {
                        spawnPoints.Add(new Point(x, y));
                        //TODO:: Check if square is empty of item and/or player
                    }
                }
            }
            return spawnPoints;
        }

        public static Point Spawn()
        {
            Random random = new Random();
            int location = random.Next(0, Map.Tiles.Length - 1);
            return GetSpawnLocations()[location];
        }
    }
}
