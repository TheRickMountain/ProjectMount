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
            Add(new Sprite(GamePlayState.TileSet.Texture, GamePlayState.TileSet.SourceRectangles[TileMap.GRASS], 16, 16, true));
            Add(new Gatherable(new Hay(), 1));
        }

    }
}
