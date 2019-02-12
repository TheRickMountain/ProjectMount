using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MountPRG
{
    public class Settler : Entity
    {


        public Settler(Vector2 position) : base(position)
        {
            Add(new Sprite(GamePlayState.SettlerTexture, true));
            Add(new SettlerController());
        }

        public void SetDestTile(Tile tile, Dictionary<Tile, Node<Tile>> nodes, TileLayer layer)
        {
            SettlerController sc = Get<SettlerController>();
            sc.SetDestTile(tile, nodes, layer);
        }

    }
}
