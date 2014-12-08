using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuissANT
{
    [Flags]
    public enum TileInfo
    {
        //TileInfo is split like this:
        //    11111111 11111111
        // First half - Pheromones/Objects
        // Second half - Tiles

        //Type
        Sky = 0x0,
        GroundSoft = 0x1,
        GroundMed = 0x2,
        GroundHard = 0x3,
        GroundImp = 0x4,
        GroundDug = 0x5,

        //Pheromone Types
        MIN_OBJECT_VALUE = 0x1 << 8,
        Nest = 0x2 << 8,
        Attack = 0x3 << 8,
        Gather = 0x4 << 8,

        //Clearing types
        CLEAR_TILE = 0xFF << 8,
        CLEAR_OBJECT = 0xFF,

    }

    public static class TileInfoSets
    {
        public static TileInfo[] PheromoneTypes = new TileInfo[]
        {
            TileInfo.Nest, 
            TileInfo.Attack, 
            //TileInfo.Gather
        };
    }

    public static class TileInfoExtentions
    {
        public static TileInfo ClearTileInfo(this TileInfo tile)
        {
            tile &= TileInfo.CLEAR_TILE;
            return tile;
        }

        public static TileInfo ClearTileObject(this TileInfo tile)
        {
            tile &= TileInfo.CLEAR_OBJECT;
            return tile;
        }

        public static TileInfo OverwriteTileValue(this TileInfo tile, TileInfo newTileType)
        {
            if (IsObjectTile(newTileType))
                tile = ClearTileObject(tile);
            else
            {
                //Make sure it's not sky
                TileInfo tileValue = tile.ClearTileObject();
                if (tileValue == TileInfo.Sky)
                    return tile;

                tile = ClearTileInfo(tile);
            }

            tile |= newTileType;
            return tile;
        }

        public static bool IsObjectTile(this TileInfo tile)
        {
            return (short)tile >= (short)TileInfo.MIN_OBJECT_VALUE;
        }

        public static bool IsTileType(this TileInfo tile, TileInfo tileType)
        {
            TileInfo val = tile;
            if (tileType.IsObjectTile())
            {
                val = val.ClearTileInfo();
            }
            else
            {
                val = val.ClearTileObject();
            }

            return tileType == val;
        }

        public static bool IsTileType(this TileInfo tile, TileInfo[] tileTypes)
        {
            TileInfo val = tile;
            if (tileTypes[0].IsObjectTile())
            {
                val = val.ClearTileInfo();
            }
            else
            {
                val = val.ClearTileObject();
            }

            foreach (TileInfo tileType in tileTypes)
            {
                if(tileType == val)
                    return true;
            }
            return false;
        }

        public static bool IsPassable(this TileInfo tile, TileInfo[] passableTiles)
        {
            return passableTiles.Contains<TileInfo>(tile); 
        }
    }
}
