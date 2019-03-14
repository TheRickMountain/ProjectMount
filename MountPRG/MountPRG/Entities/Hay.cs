using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Hay : Entity
    {

        public Hay()
        {
            Add(new Sprite(GamePlayState.TileSet.Texture, GamePlayState.TileSet.SourceRectangles[TileMap.HAY], 16, 16, true));
            Add(new Gatherable(ItemDatabase.GetItemById(TileMap.HAY), 1));
        }

    }
}
