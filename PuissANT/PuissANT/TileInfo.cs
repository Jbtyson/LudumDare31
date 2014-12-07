using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuissANT
{
    [Flags]
    public enum TileInfo
    {
        //Type 0x1 - 0x8
        Sky = 0x1,
        Surface = 0x2,
        Ground_Undug = 0x4,
        Ground_Dug = 0x8,

        //Objects 0x10 - 0x18


        //Pheromone Types 0x20 - 0x28
        Nest = 0x20,
        Attack = 0x21,
    }

    public static class TileInfoExtentions
    {
        
    }
}
