using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandServer
{
    public class Game
    {
        public int Turn { get; set; }
        public List<KeyValuePair<Guid, Action>> ActionQueue { get; set; }

        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public int MapSeed { get; set; }
        public int[][] MapTiles { get; set; }
        public List<Player> Players { get; set; }
        public bool Started = false;

        public Game(int mapWidth, int mapHeight, int mapSeed)
        {
            Players = new List<Player>();
            ActionQueue = new List<KeyValuePair<Guid, Action>>();

            MapHeight = mapHeight;
            MapWidth = mapWidth;
            Map map = new Map(mapWidth, mapHeight);
            map.Seed = mapSeed;
            map.Generate();
            MapTiles = map.Tiles;
        }

        public List<Point> GetSpawnLocations()
        {
            List<Point> spawnPoints = new List<Point>();
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    if (MapTiles[y][x] == (int)Tile.Spawn)
                    {
                        spawnPoints.Add(new Point(x, y));
                        //TODO:: Check if square is empty of item and/or player
                    }
                }
            }
            return spawnPoints;
        }

        public Point Spawn()
        {
            Random random = new Random();
            int location = random.Next(0, MapTiles.Length - 1);
            return GetSpawnLocations()[location];
        }
    }
}
