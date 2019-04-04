using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Grass : Entity
    {

        public Grass()
        {
            Add(new SpriteCmp(GamePlayState.TileSet.Texture, GamePlayState.TileSet.SourceRectangles[TileMap.GRASS], 16, 16));
            Add(new GatherableCmp(GamePlayState.ItemDatabase[Item.HAY], 1));
        }

    }
}
