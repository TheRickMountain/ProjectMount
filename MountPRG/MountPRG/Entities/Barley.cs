using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Barley : Entity
    {

        public Barley()
        {
            Add(new SpriteCmp(GamePlayState.TileSet.Texture, GamePlayState.TileSet.SourceRectangles[TileMap.BARLEY], 16, 16));
            Add(new GatherableCmp(ItemDatabase.GetItemById(TileMap.BARLEY_SEED), 1));
        }

    }
}
