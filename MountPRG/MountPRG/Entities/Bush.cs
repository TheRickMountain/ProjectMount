using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class Bush : Entity
    {

        public Bush()
        {

            SpriteCmp sprite = new SpriteCmp(GamePlayState.TileSet.Texture, GamePlayState.TileSet.SourceRectangles[TileMap.RASPBERRY_BUSH], 16, 16);
            Add(sprite);

            GatherableCmp gatherable = new GatherableCmp(GamePlayState.ItemDatabase[Item.BERRY], 1, true);
            Add(gatherable);
            gatherable.OnCountEqualsZeroCallback(delegate
            {
                sprite.Source = GamePlayState.TileSet.SourceRectangles[TileMap.BUSH];
            });
        }

    }
}
