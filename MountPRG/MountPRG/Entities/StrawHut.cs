using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MountPRG
{
    public class StrawHut : Entity
    {

        public StrawHut()
        {
            Add(new Sprite(GamePlayState.TileSet.Texture, new Rectangle(0, 16, 32, 32), 32, 32, true));
            Add(new Building(2, 2));
            Add(new Hut());
        }

    }
}
