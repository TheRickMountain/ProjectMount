using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Flint : Entity
    {

        public Flint() {
            Add(new Sprite(GamePlayState.TileSet.Texture, GamePlayState.TileSet.SourceRectangles[TileMap.FLINT], 16, 16, true));
            Add(new Gatherable(ItemDatabase.GetItemById(TileMap.FLINT),1)); 
        }

    }
}
