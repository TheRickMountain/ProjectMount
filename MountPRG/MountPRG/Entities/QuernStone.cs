using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MountPRG
{
    public class QuernStone : Entity
    {

        public QuernStone()
        {
            Add(new SpriteCmp(GamePlayState.TileSet.Texture, new Rectangle(80, 32, 16, 16), 16, 16));
            BuildingCmp buildingCmp = new BuildingCmp(1, 1);
            buildingCmp.AddRequiredResource(GamePlayState.ItemDatabase[Item.STONE], 1);
            Add(buildingCmp);
        }

    }
}
