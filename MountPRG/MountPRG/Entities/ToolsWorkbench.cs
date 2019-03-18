using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MountPRG
{
    public class ToolsWorkbench : Entity
    {

        public ToolsWorkbench()
        {
            Add(new Sprite(GamePlayState.TileSet.Texture, new Rectangle(32, 16, 32, 32), 32, 32, true));
            Add(new Building(2, 2));
        }

    }
}
