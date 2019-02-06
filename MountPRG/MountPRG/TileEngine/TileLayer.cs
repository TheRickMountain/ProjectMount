using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MountPRG.Entities;

namespace MountPRG.TileEngine
{
    public struct Tile
    {
        public int Id { get; set; }
        public Entity Entity { get; set; }
    }

    public class TileLayer
    {

        private Tile[] tiles;

        private int width;
        private int height;

        public int Width
        {
            get { return width; }
            private set { width = value; }
        }

        public int Height
        {
            get { return height; }
            private set { height = value; }
        }

        public bool Visible { get; set; }


        public TileLayer(int width, int height, int fill)
        {
            Visible = true;

            tiles = new Tile[height * width];
            this.width = width;
            this.height = height;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    tiles[y * width + x].Id = fill;
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0)
                throw new Exception("Выход за пределы TileMap");

            if (x >= width || y >= height)
                throw new Exception("Выход за пределы TileMap");

            return tiles[y * width + x];
        }

        public void SetTile(int x, int y, int tileIndex)
        {
            if (x < 0 || y < 0)
                return;

            if (x >= width || y >= height)
                return;

            tiles[y * width + x].Id = tileIndex;
        }

        public void SetEntity(int x, int y, Entity entity)
        {
            tiles[y * width + x].Entity = entity;
        }

        public Entity GetEntity(int x, int y)
        {
            return tiles[y * width + x].Entity;
        }

        public void RemoveEntity(int x, int y)
        {
            tiles[y * width + x].Entity = null;
        }

        public bool HasEntity(int x, int y)
        {
            return tiles[y * width + x].Entity != null;
        }

    }

}
