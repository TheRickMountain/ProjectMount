using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Stick : Entity
    {

        public Stick() {
            Add(new Sprite(GamePlayState.TileSet.Texture, GamePlayState.TileSet.SourceRectangles[TileMap.STICK], 16, 16, true));
            Add(new Gatherable(ItemDatabase.GetItemById(TileMap.STICK), 1)); 
        }

    }
}
