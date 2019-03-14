using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Stick : Entity
    {
        public Stick()
        {
            Id = TileMap.STICK;
            Add(new Sprite(GamePlayState.TileSet.Texture, GamePlayState.TileSet.SourceRectangles[TileMap.STICK], 16, 16, true));
            Add(new Gatherable(this, 1));
        }
    }
}
