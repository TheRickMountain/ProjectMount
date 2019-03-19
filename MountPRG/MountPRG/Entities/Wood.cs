using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Wood : Entity
    {
        public Wood()
        {
            Id = TileMap.WOOD;
            Add(new Sprite(GamePlayState.TileSet.Texture, GamePlayState.TileSet.SourceRectangles[TileMap.WOOD], 16, 16, true));
            Add(new Gatherable(this, 1));
        }
    }
}
