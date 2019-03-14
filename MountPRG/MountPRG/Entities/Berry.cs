using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Berry : Entity
    {
        public Berry()
        {
            Id = TileMap.BERRY;
            Add(new Sprite(GamePlayState.TileSet.Texture, GamePlayState.TileSet.SourceRectangles[TileMap.BERRY], 16, 16, true));
            Add(new Gatherable(this, 1));
        }
    }
}
