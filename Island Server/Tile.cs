using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class Tile
    {
        public TileType Type { get; set; }
        public string Colour { get; set; }
    }

    public enum TileType
    {
        DeepWater,
        MediumWater,
        ShallowWater,
        Spawn,
        Sand,
        Grass,
        Foliage,
        Tree,
        Dirt,
        Rock
    }
}
